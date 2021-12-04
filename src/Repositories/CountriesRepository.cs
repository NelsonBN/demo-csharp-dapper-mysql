using Dapper;
using WebAPI.Dapper.Mysql.DTOs.Countries;
using WebAPI.Dapper.Mysql.Helpers.Interfaces;

namespace WebAPI.Dapper.Mysql.Repositories;

public class CountriesRepository
{
    private readonly IDBContext _context;
    public CountriesRepository(IDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CountryDTO>> GetAllAsync()
    {
        var result = await _context.Connection.QueryAsync<CountryDTO>(@"
            SELECT
                `iso2`,
                `iso3`,
                `iso_numeric` AS `ISONumeric`,
                `english_name` AS `CountryName`,
                `native_name` AS `NativeName`,
                `nationality_name` AS `NationalityName`,
                `capital`,
                `currency_code`,
                `domain`,
                `continent_id` AS `ContinentId`
            FROM `country` ;
        ");

        return result;
    }

    public async Task<CountryDTO> GetAsync(string iso2)
    {
        var result = await _context.Connection.QuerySingleOrDefaultAsync<CountryDTO>(@"
            SELECT
                `iso2`,
                `iso3`,
                `iso_numeric` AS `ISONumeric`,
                `english_name` AS `CountryName`,
                `native_name` AS `NativeName`,
                `nationality_name` AS `NationalityName`,
                `capital`,
                `currency_code`,
                `domain`,
                `continent_id` AS `ContinentId`
            FROM `country` 
            WHERE iso2 = @iso2 ;",

            new { iso2 } // This is equals `new { iso2 = iso2 }`
        );

        return result;
    }

    public async Task<IEnumerable<CountryDTO>> GetAsync(IPagination pagination)
    {
        var result = await _context.Connection.QueryAsync<CountryDTO>($@"
            SELECT
                `iso2`,
                `iso3`,
                `iso_numeric` AS `ISONumeric`,
                `english_name` AS `CountryName`,
                `native_name` AS `NativeName`,
                `nationality_name` AS `NationalityName`,
                `capital`,
                `currency_code`,
                `domain`,
                `continent_id` AS `ContinentId`
            FROM `country`

            ORDER BY {pagination.Sort} {pagination.Direction}

            LIMIT @Offset, @Limit ;
        ", pagination);

        return result;
    }

    public async Task<IEnumerable<CountryWithContinentDTO>> GetFullAsync()
    {
        var result = await _context.Connection.QueryAsync<CountryWithContinentDTO, ContinentDTO, CountryWithContinentDTO>(
            sql: @"
                SELECT
                    `iso2`,
                    `iso3`,
                    `iso_numeric` AS `ISONumeric`,
                    `country`.`english_name` AS `CountryName`,
                    `native_name` AS `NativeName`,
                    `nationality_name` AS `NationalityName`,
                    `capital`,
                    `currency_code`,
                    `domain`,

                    `continent`.`continent_id` AS `ContinentId`,
                    `continent`.`english_name` AS `ContinentName`
                FROM `country`
                INNER JOIN `continent` ON `continent`.`continent_id` = `country`.`continent_id` ;
            ",
            map: (country, continent) =>
            {
                country.Continent = continent;
                return country;
            },
            splitOn: "ContinentId"
        );

        return result;
    }

    public async Task<SearchDTO> SearchAsync(string search)
    {
        const string QUERY_SEARCH = @"WHERE
            `english_name`     LIKE CONCAT('%', @search, '%') OR
            `native_name`      LIKE CONCAT('%', @search, '%') OR
            `nationality_name` LIKE CONCAT('%', @search, '%')
        ";

        const string SQL = $@"
            SELECT COUNT(*) FROM `country` ;
            SELECT COUNT(*) FROM `country` {QUERY_SEARCH} ;
            SELECT
                `iso2`,
                `iso3`,
                `iso_numeric` AS `ISONumeric`,
                `english_name` AS `CountryName`,
                `native_name` AS `NativeName`,
                `nationality_name` AS `NationalityName`,
                `capital`,
                `currency_code`,
                `domain`,
                `continent_id` AS `ContinentId`
            FROM `country`
            {QUERY_SEARCH} ;
        ";

        using (var reader = await _context.Connection.QueryMultipleAsync(SQL, new { search }))
        {
            return new SearchDTO
            {
                Total = await reader.ReadSingleAsync<long>(),
                TotalFiltered = await reader.ReadSingleAsync<long>(),

                Countries = await reader.ReadAsync<CountryDTO>()
            };
        }
    }
}