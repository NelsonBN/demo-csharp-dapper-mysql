namespace WebAPI.Dapper.Mysql.DTOs.Persons;

public record PersonRequest
{
    private uint MIN_AGE = 18;

    public string Name { get; set; } = default!;
    public uint Age { get; set; }

    public string? Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            return $"The {nameof(Name)} cannot be null or empty";
        }

        if (Age < MIN_AGE)
        {
            return $"The {nameof(Age)} must be less than {MIN_AGE}";
        }

        return null;
    }
}