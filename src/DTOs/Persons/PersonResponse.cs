namespace WebAPI.Dapper.Mysql.DTOs.Persons;

public record PersonResponse : PersonRequest
{
    public uint Id { get; set; }
}