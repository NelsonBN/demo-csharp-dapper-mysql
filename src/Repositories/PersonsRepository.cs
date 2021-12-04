using Dapper;
using WebAPI.Dapper.Mysql.DTOs.Persons;
using WebAPI.Dapper.Mysql.Helpers.Interfaces;

namespace WebAPI.Dapper.Mysql.Repositories;

public class PersonsRepository
{
    private readonly IDBContext _context;
    public PersonsRepository(IDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PersonResponse>> GetAsync()
    {
        return await _context.Connection.QueryAsync<PersonResponse>(@"
            SELECT
                `id`,
                `name`,
                `age`
            FROM `person` ;
        ");
    }

    public async Task<PersonResponse> GetAsync(uint id)
    {
        return await _context.Connection.QuerySingleOrDefaultAsync<PersonResponse>(@"
            SELECT
                `id`,
                `name`,
                `age`
            FROM `person`
            WHERE `id` = @id;
        ",
        new { id });
    }

    public async Task<uint> AddAsync(PersonRequest person)
    {
        return await _context.Connection.QuerySingleAsync<uint>(@"
            INSERT `person` (`name`, `age`)
                      VALUE (@Name , @Age );
            SELECT CAST(LAST_INSERT_ID() AS UNSIGNED INTEGER);",
            person
        );
    }

    public async Task<bool> UpdateAsync(uint id, PersonRequest person)
    {
        return await _context.Connection.ExecuteAsync(@"
            UPDATE `person` SET
                `name` = @Name,
                `age`  = @Age
            WHERE `id` = @id ;",
            new { id, person.Name, person.Age }
        ) > 0;
    }

    public async Task<bool> DeleteAsync(uint id)
    {
        return await _context.Connection.ExecuteAsync(@"
            DELETE FROM `person`
                WHERE `id` = @id ;",
            new { id }
        ) > 0;
    }
}