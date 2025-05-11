namespace Tutorial9.Exceptions;

public class WarehouseException : Exception
{
    public WarehouseException(string? message) : base(message){}
}
public class ProductNotFoundException : WarehouseException
{
    public ProductNotFoundException(string message) : base(message) { }
}

public class WarehouseNotFoundException : WarehouseException
{
    public WarehouseNotFoundException(string message) : base(message) { }
}

public class InvalidAmountException : WarehouseException
{
    public InvalidAmountException(string message) : base(message) { }
}

public class OrderNotFoundException : WarehouseException
{
    public OrderNotFoundException(string message) : base(message) { }
}

public class OrderAlreadyCompletedException : WarehouseException
{
    public OrderAlreadyCompletedException(string message) : base(message) { }
}

public class DataBaseException : WarehouseException
{
    public DataBaseException(string message) : base(message) { }
}