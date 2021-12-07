namespace WebAPI.Dapper.Mysql.DTOs.Products;

public record ProductResponse
{
    public uint Id { get; protected set; } // To test the deserialization with dapper with protected set properties
    public string Description { get; init; } = default!; // To test the deserialization with dapper with init properties
    public uint Quantity { get; private set; } // To test the deserialization with dapper with private set properties

    private ProductResponse()
    { // To test the deserialization with dapper without a parameterless constructor
    }
}