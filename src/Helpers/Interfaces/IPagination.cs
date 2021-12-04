namespace WebAPI.Dapper.Mysql.Helpers.Interfaces;

public interface IPagination
{
    int Offset { get; init; }
    int Limit { get; init; }

    string Sort { get; init; }
    string Direction { get; init; }

    bool Validate();
}