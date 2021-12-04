namespace WebAPI.Dapper.Mysql.DTOs.Countries;

public record ContinentDTO
{
    public ushort ContinentId { get; set; }
    public string? ContinentName { get; set; }
}