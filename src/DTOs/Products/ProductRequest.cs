namespace WebAPI.Dapper.Mysql.DTOs.Products;

public record ProductRequest
{
    private uint MIN_QUANTITY = 1;

    public string Description { get; set; } = default!;
    public uint Quantity { get; set; }

    public string? Validate()
    {
        if (string.IsNullOrWhiteSpace(Description))
        {
            return $"The {nameof(Description)} cannot be null or empty";
        }

        if (Quantity < MIN_QUANTITY)
        {
            return $"The {nameof(Quantity)} must be less than {MIN_QUANTITY}";
        }

        return null;
    }
}