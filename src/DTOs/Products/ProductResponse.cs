namespace WebAPI.Dapper.Mysql.DTOs.Products;

public record ProductResponse : ProductRequest
{
    public uint Id { get; set; }
}