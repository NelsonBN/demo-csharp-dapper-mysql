using System.ComponentModel;
using WebAPI.Dapper.Mysql.Helpers.Interfaces;

namespace WebAPI.Dapper.Mysql.Repositories;

public record Pagination<TResponse> : IPagination
{
    public const string ASC = "ASC";
    public const string DESC = "DESC";
    public const string DEFAULT = ASC;

    [DefaultValue(0)]
    public int Offset { get; init; }

    [DefaultValue(100)]
    public int Limit { get; init; }

    public string Sort { get; init; }

    [DefaultValue(DEFAULT)]
    public string Direction { get; init; }


    private readonly string[] _fields;

    public Pagination()
    {
        _fields = typeof(TResponse)
            .GetProperties()
            .Select(s => s.Name)
            .ToArray();

        Direction = DEFAULT;

        Sort = _fields[0]; // Get the first property as a default value
    }

    public bool Validate()
    {
        // Pagination validation
        if(Offset < 0)
        {
            return false;
        }

        if(Limit < 1)
        {
            return false;
        }
        if(Limit > 100)
        {
            return false;
        }

        // Sort validations
        if(_fields.Any(a => a.Contains(Sort, StringComparison.InvariantCultureIgnoreCase)) is false)
        {
            return false;
        }

        if(
            ASC.Equals(Direction, StringComparison.InvariantCultureIgnoreCase) is false
            &&
            DESC.Equals(Direction, StringComparison.InvariantCultureIgnoreCase) is false
        )
        {
            return false;
        }

        return true;
    }
}