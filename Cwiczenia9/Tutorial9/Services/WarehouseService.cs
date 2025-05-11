using System.Data;
using Microsoft.Data.SqlClient;
using Tutorial9.Exceptions;
using Tutorial9.Model;
namespace Tutorial9.Services;

public class WarehouseService: IWarehouseService
{
    private readonly string _connectionString;
    public WarehouseService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default") ?? string.Empty;
    }

    public async Task<int> AddProductToWarehouseAsync(WarehouseRequestDTO request)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        using var transaction = connection.BeginTransaction();

        try
        {
            //Sprawdzenie czy produkt istnieje
            var checkProductExistCommand = new SqlCommand("SELECT COUNT(*) FROM Product WHERE IdProduct = @IdProduct",
                connection, transaction);
            checkProductExistCommand.Parameters.AddWithValue("@IdProduct", request.IdProduct);
            var checkProductExistResult = (int)await checkProductExistCommand.ExecuteScalarAsync() > 0;
            if (!checkProductExistResult)
            {
                throw new ProductNotFoundException("Product not found.");
            }

            //Sprawdzenie czy magazyn istnieje
            var checkWarehouseExistCommand =
                new SqlCommand("SELECT COUNT(*) FROM Warehouse WHERE IdWarehouse = @IdWarehouse", connection,
                    transaction);
            checkWarehouseExistCommand.Parameters.AddWithValue("@IdWarehouse", request.IdWarehouse);
            var checkWarehouseExistResult = (int)await checkWarehouseExistCommand.ExecuteScalarAsync() > 0;
            if (!checkWarehouseExistResult)
            {
                throw new WarehouseNotFoundException("Warehouse not found.");
            }

            //Sprawdzenie czy przekazana ilosc jest wieksza od 0
            if (request.Amount <= 0)
            {
                throw new InvalidAmountException("Amount must be greater than 0.");
            }

            //Sprawdzenie czy zamowienie zakupu istnieje w tabeli Order
            var checkOrderExist =
                new SqlCommand(
                    "SELECT TOP 1 IdOrder, CreatedAt FROM [Order] WHERE IdProduct = @IdProduct AND Amount =@Amount AND CreatedAt < @CreatedAt",
                    connection, transaction);
            checkOrderExist.Parameters.AddWithValue("@IdProduct", request.IdProduct);
            checkOrderExist.Parameters.AddWithValue("@Amount", request.Amount);
            checkOrderExist.Parameters.AddWithValue("@CreatedAt", request.CreatedAt);

            int? orderId = null;
            DateTime? orderCreatedAt = null;
            using (var reader = await checkOrderExist.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    orderId = reader.GetInt32(0);
                }
            }

            if (!orderId.HasValue)
            {
                throw new OrderNotFoundException("Order not found.");
            }

            //Sprawdzenie czy zamowienie zostalo zrealizowane
            var checkIfCompletedOrderCommand =
                new SqlCommand("SELECT COUNT(*) FROM Product_Warehouse WHERE IdOrder = @IdOrder", connection,
                    transaction);
            checkIfCompletedOrderCommand.Parameters.AddWithValue("@IdOrder", orderId.Value);
            var checkIfCompletedOrderResult = (int)await checkIfCompletedOrderCommand.ExecuteScalarAsync() > 0;
            if (checkIfCompletedOrderResult)
            {
                throw new OrderAlreadyCompletedException("Order already completed.");
            }

            //Aktualizujemy kolumne FullfilledAt
            var updateFullfillerDate =
                new SqlCommand("UPDATE [ORDER] SET FulfilledAt = GETDATE() WHERE IdOrder = @IdOrder", connection,
                    transaction);
            updateFullfillerDate.Parameters.AddWithValue("@IdOrder", orderId.Value);
            await updateFullfillerDate.ExecuteNonQueryAsync();

            //Pobranie ceny produktu
            var getProductPriceCommand = new SqlCommand("SELECT Price FROM Product WHERE IdProduct = @IdProduct",
                connection, transaction);
            getProductPriceCommand.Parameters.AddWithValue("@IdProduct", request.IdProduct);
            var price = (decimal)await getProductPriceCommand.ExecuteScalarAsync();
            var totalPrice = price * request.Amount;

            //Dodanie rekordu do tabeli Product_Warehouse i dodanie daty utworzenia
            var insertProductWarehouse = new SqlCommand(
                "INSERT INTO Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) OUTPUT INSERTED.IdProductWarehouse VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, GETDATE())",
                connection, transaction);
            insertProductWarehouse.Parameters.AddWithValue("@IdWarehouse", request.IdWarehouse);
            insertProductWarehouse.Parameters.AddWithValue("@IdProduct", request.IdProduct);
            insertProductWarehouse.Parameters.AddWithValue("@IdOrder", orderId.Value);
            insertProductWarehouse.Parameters.AddWithValue("@Amount", request.Amount);
            insertProductWarehouse.Parameters.AddWithValue("@Price", totalPrice);
            var insertProductWarehouseResult = (int)await insertProductWarehouse.ExecuteScalarAsync();
            await transaction.CommitAsync();
            return insertProductWarehouseResult;

        }
        catch (SqlException exception)
        {
            await transaction.RollbackAsync();
            throw new DataBaseException($"Database error: {exception.Message}");
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<int> AddProductToWarehouseProcedure(WarehouseRequestDTO request)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("AddProductToWarehouse", connection);
        command.CommandType = CommandType.StoredProcedure;
        
        command.Parameters.AddWithValue("@IdProduct", request.IdProduct);
        command.Parameters.AddWithValue("@IdWarehouse", request.IdWarehouse);
        command.Parameters.AddWithValue("@Amount", request.Amount);
        command.Parameters.AddWithValue("@CreatedAt", request.CreatedAt);
        await connection.OpenAsync();
        var result = await command.ExecuteNonQueryAsync(); 
        return Convert.ToInt32(result);
    }
}