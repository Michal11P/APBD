using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public class ClientService : IClientService
{
    private readonly string _connectionString =
        "Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;TrustServerCertificate=True;";
    
    public async Task<List<ClientTripDTO>> GetClientTrip(int id)
    {
        var clientTrips = new List<ClientTripDTO>();

        //Zapytanie SQL  pobierajace liste wycieczek, w ktorych uczestniczy klient o podanym ID
        var query = @"
           SELECT t.IdTrip, t.Name, t.Description, t.DateFrom, t.DateTo, t.MaxPeople,
                  ct.RegisteredAt, ct.PaymentDate
           FROM Client c
           JOIN Client_Trip ct ON c.IdClient = ct.IdClient
           JOIN Trip t ON ct.IdTrip = t.IdTrip
           WHERE c.IdClient = @IdClient";

        using (var conn = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@IdClient", id);

            await conn.OpenAsync();
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (!reader.HasRows)
                {
                    return clientTrips; // Zwracamy pustą listę, jeżeli klient nie ma wycieczek lub nie istnieje
                }

                while (await reader.ReadAsync())
                {
                    clientTrips.Add(new ClientTripDTO
                    {
                        IdTrip = reader.GetInt32(reader.GetOrdinal("IdTrip")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        DateFrom = reader.GetDateTime(reader.GetOrdinal("DateFrom")),
                        DateTo = reader.GetDateTime(reader.GetOrdinal("DateTo")),
                        MaxPeople = reader.GetInt32(reader.GetOrdinal("MaxPeople")),
                        RegisteredAt = reader.GetInt32(reader.GetOrdinal("RegisteredAt")),
                        PaymentDate = reader.IsDBNull(reader.GetOrdinal("PaymentDate"))
                            ? (int?)null
                            : reader.GetInt32(reader.GetOrdinal("PaymentDate"))
                    });
                }
            }
        }

        return clientTrips;
    }
    //Dodaje nowego klientado bazy danych i zwraca jego ID
    public async Task<int> AddClient(ClientDTO client)
    {
        
        var command = @"INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel)
                    OUTPUT INSERTED.IdClient 
                    VALUES (@FirstName, @LastName, @Email, @Telephone, @Pesel)";

        using (var conn = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@FirstName", client.FirstName);
            cmd.Parameters.AddWithValue("@LastName", client.LastName);
            cmd.Parameters.AddWithValue("@Email", client.Email);
            cmd.Parameters.AddWithValue("@Telephone", client.Telephone);
            cmd.Parameters.AddWithValue("@Pesel", client.Pesel);
            await conn.OpenAsync();
            var newId = (int)await cmd.ExecuteScalarAsync();
            return newId;
        }
    }

    //Rejestruje klienta o podanym ID do wycieczki o tripID
    public async Task<RegisterTripResult> RegisterClientToTrip(int id, int tripId)
    {
        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        
        //Sprawdza czy klient istnieje
        var clientExistCommand =new SqlCommand("SELECT COUNT(*) FROM Client WHERE IdClient = @IdClient", conn);
        clientExistCommand.Parameters.AddWithValue("@IdClient", id);
        var clientExistResult = (int)await clientExistCommand.ExecuteScalarAsync()>0;
        if (!clientExistResult)
        {
            return RegisterTripResult.ClientNotFound;
        }
        
        //Sprawdza czy wycieczka istnieje
        var tripExistCommand = new SqlCommand("SELECT COUNT(*) FROM Trip WHERE IdTrip = @IdTrip", conn);
        tripExistCommand.Parameters.AddWithValue("@IdTrip", tripId);
        var tripExistResult = (int)await tripExistCommand.ExecuteScalarAsync()>0;
        if (!tripExistResult)
        {
            return RegisterTripResult.TripNotFound;
        }

        //Sprawdza czy klient jest juz zapisany na te wycieczke
        var alreadyRegisteredCommand = new SqlCommand("SELECT COUNT(1) FROM Client_Trip WHERE IdClient = @ClientId AND IdTrip = @TripId", conn);
        alreadyRegisteredCommand.Parameters.AddWithValue("@ClientId", id);  
        alreadyRegisteredCommand.Parameters.AddWithValue("@TripId", tripId);
        var alreadyRegisteredResult = (int)await alreadyRegisteredCommand.ExecuteScalarAsync()>0;
        if (alreadyRegisteredResult)
        {
            return RegisterTripResult.AlreadyRegistered;
        }
        
        //Sprawdzanie liczby uczestnikow zapisanych na wycieczke
        var countCommand = new SqlCommand("Select Count(*) FROM Client_Trip WHERE IdTrip=@TripId", conn);
        countCommand.Parameters.AddWithValue("@TripId", tripId);
        var countResult = (int)await countCommand.ExecuteScalarAsync();
        
        //Sprawdzenie maksymalnej liczby uczestnikow wycieczki
        var maxCountCommand = new SqlCommand("Select MaxPeople FROM Trip WHERE IdTrip = @TripId", conn);
        maxCountCommand.Parameters.AddWithValue("@TripId", tripId);
        var maxCountResult = (int)await maxCountCommand.ExecuteScalarAsync();
        if (countResult >= maxCountResult)
        {
            return RegisterTripResult.TripFull;
        }
        
        //Dodanie wpisu do tabeli Client_Trip z aktualnym timestampem
        var insertCommand = new SqlCommand(@"INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt) VALUES (@IdClient, @IdTrip, @RegisteredAt)", conn);
        insertCommand.Parameters.AddWithValue("@IdClient", id); 
        insertCommand.Parameters.AddWithValue("@IdTrip", tripId);
        var unixTime = (int)DateTimeOffset.Now.ToUnixTimeSeconds();
        insertCommand.Parameters.AddWithValue("@RegisteredAt", unixTime);
        await insertCommand.ExecuteNonQueryAsync();

        return RegisterTripResult.Success;

    }

   //Wypisuje klienta o podanym ID z wycieczki o tripID
    public async Task<DeleteTripResult> UnregisterClientFromTrip(int id, int tripId)
    {
        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        
        //Sprawdza czy klient istnieje
        var clientExistCommand =new SqlCommand("SELECT COUNT(*) FROM Client WHERE IdClient = @IdClient", conn);
        clientExistCommand.Parameters.AddWithValue("@IdClient", id);
        var clientExistResult = (int)await clientExistCommand.ExecuteScalarAsync()>0;
        if (!clientExistResult)
        {
            return DeleteTripResult.ClientNotFound;
        }
        
        //Sprawdza czy wycieczka istnieje
        var tripExistCommand = new SqlCommand("SELECT COUNT(*) FROM Trip WHERE IdTrip = @IdTrip", conn);
        tripExistCommand.Parameters.AddWithValue("@IdTrip", tripId);
        var tripExistResult = (int)await tripExistCommand.ExecuteScalarAsync()>0;
        if (!tripExistResult)
        {
            return DeleteTripResult.TripNotFound;
        }
        
        //Sprawdza czy klient jest juz zapisany na wycieczke
        var registrationExistCommand = new SqlCommand("Select Count(*) FROM Client_Trip WHERE IdClient = @IdClient AND IdTrip = @TripId", conn);
        registrationExistCommand.Parameters.AddWithValue("@IdClient", id);
        registrationExistCommand.Parameters.AddWithValue("@TripId", tripId);
        var registrationResult = (int)await registrationExistCommand.ExecuteScalarAsync()>0;
        if (!registrationResult)
        {
            return DeleteTripResult.RegistrationNotFound;
        }
        
        //Wypisuje klient o podanym ID z wycieczki o tripID
        var deleteCmd = new SqlCommand("DELETE FROM Client_Trip WHERE IdClient = @ClientId AND IdTrip = @TripId", conn);
        deleteCmd.Parameters.AddWithValue("@ClientId", id);
        deleteCmd.Parameters.AddWithValue("@TripId", tripId);
        await deleteCmd.ExecuteNonQueryAsync();

        return DeleteTripResult.Success;
        
    }

}