using MySql.Data.MySqlClient;
using System.Data;
using WebAPI.Dapper.Mysql.Helpers.Interfaces;

namespace WebAPI.Dapper.Mysql.Helpers;

public sealed class DBContext : IDBContext
{
    public IDbConnection Connection { get; init; }
    public IDbTransaction Transaction { get; private set; }

    public DBContext(
        ILogger<DBContext> logger,
        IConfiguration configuration
    )
    {
        var dbHost = configuration.GetSection("DB_HOST").Value;
        var connectionString = $"server={dbHost};database=demo_db;uid=root";

        // logger.LogInformation($">>> CONNECTION STRING: '{connectionString}'");

        Connection = new MySqlConnection(connectionString);

        Connection.Open();
    }

    public void BeginTransaction() =>
        Transaction = Connection.BeginTransaction();

    public void Dispose() => Connection?.Dispose();
}