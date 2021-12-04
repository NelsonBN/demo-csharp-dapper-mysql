using System.Data;
using WebAPI.Dapper.Mysql.Helpers.Interfaces;

namespace WebAPI.Dapper.Mysql.Helpers;

public sealed class UnitOfWork : IUnitOfWork
{
    public IDbTransaction? Transaction { get; private set; }

    public UnitOfWork(IDBContext context)
    {
        Transaction = context.Connection.BeginTransaction();
    }

    public void Commit()
    {
        try
        {
            Transaction?.Commit();
            Transaction?.Connection?.Close();
        }
        catch
        {
            Transaction?.Rollback();
            throw;
        }
    }

    public void Rollback()
    {
        try
        {
            Transaction?.Rollback();
            Transaction?.Connection?.Close();
        }
        catch
        {
            throw;
        }
    }

    public void Dispose()
    {
        Transaction?.Dispose();
        Transaction?.Connection?.Dispose();
        Transaction = null;
    }
}