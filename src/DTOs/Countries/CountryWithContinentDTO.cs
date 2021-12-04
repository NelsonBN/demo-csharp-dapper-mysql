using System.Text.Json.Serialization;

namespace WebAPI.Dapper.Mysql.DTOs.Countries;

public record CountryWithContinentDTO
{
    [JsonPropertyName("iso2")]
    public string? ISO2 { get; set; }
    [JsonPropertyName("iso3")]
    public string? ISO3 { get; set; }
    public ushort ISONumeric { get; set; }
    public string? CountryName { get; set; }
    public string? NativeName { get; set; }
    public string? NationalityName { get; set; }
    public string? Capital { get; set; }
    public string? CurrencyCode { get; set; }
    public string? Domain { get; set; }
    public ContinentDTO Continent { get; set; } = new ContinentDTO();
}