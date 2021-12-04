namespace WebAPI.Dapper.Mysql.DTOs.EventSourcing;

public record EventSourcingDTO
{
    public uint Id { get; set; }
    public string Log { get; set; } = default!;
    public DateTime DateTime { get; set; }
}