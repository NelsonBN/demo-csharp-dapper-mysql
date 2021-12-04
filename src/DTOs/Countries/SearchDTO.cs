namespace WebAPI.Dapper.Mysql.DTOs.Countries;

public record SearchDTO
{
    public long Total { get; set; }
    public long TotalFiltered { get; set; }

    public IEnumerable<CountryDTO> Countries { get; set; } = default!;
}