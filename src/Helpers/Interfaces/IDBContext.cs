using System.Data;

namespace WebAPI.Dapper.Mysql.Helpers.Interfaces;

public interface IDBContext : IDisposable
{
    IDbConnection Connection { get; init; }
}