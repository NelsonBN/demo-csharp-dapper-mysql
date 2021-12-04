using System.Data;

namespace WebAPI.Dapper.Mysql.Helpers;

public interface IUnitOfWork : IDisposable
{
    IDbTransaction? Transaction { get; }

    void Commit();
    void Rollback();
}