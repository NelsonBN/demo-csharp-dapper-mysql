using Dapper;
using WebAPI.Dapper.Mysql.DTOs.EventSourcing;
using WebAPI.Dapper.Mysql.Helpers;

namespace WebAPI.Dapper.Mysql.Repositories;

public class EventSourcingRepository
{
    private readonly IUnitOfWork _uof;
    public EventSourcingRepository(IUnitOfWork uof)
    {
        _uof = uof;
    }

    public async Task<IEnumerable<EventSourcingDTO>> GetAsync()
    {
        return await _uof.Transaction.Connection.QueryAsync<EventSourcingDTO>(@"
            SELECT
                `id`,
                `log`,
                `datetime`
            FROM `event_sourcing`
            ORDER BY `datetime` DESC;
        ");
    }

    public async Task AddAsync(string log)
    {
        await _uof.Transaction.Connection.ExecuteAsync(@"
            INSERT `event_sourcing` (`log`, `datetime`)
                              VALUE (@log , @DateTime );",
            new { log, DateTime = DateTime.UtcNow},
            _uof.Transaction
        );
    }
}