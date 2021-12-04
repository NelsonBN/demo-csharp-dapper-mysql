using WebAPI.Dapper.Mysql.DTOs.Products;
using WebAPI.Dapper.Mysql.Helpers;
using Dapper;

namespace WebAPI.Dapper.Mysql.Repositories;

public class ProductsRepository
{
    private readonly IUnitOfWork _uof;
    public ProductsRepository(IUnitOfWork uof)
    {
        _uof = uof;
    }

    public async Task<IEnumerable<ProductResponse>> GetAsync()
    {
        return await _uof.Transaction.Connection.QueryAsync<ProductResponse>(@"
            SELECT
                `id`,
                `description`,
                `quantity`
            FROM `product` ;
        ");
    }

    public async Task<ProductResponse> GetAsync(uint id)
    {
        return await _uof.Transaction.Connection.QuerySingleOrDefaultAsync<ProductResponse>(@"
            SELECT
                `id`,
                `description`,
                `quantity`
            FROM `product`
            WHERE `id` = @id;
        ",
        new { id });
    }

    public async Task<uint> AddAsync(ProductRequest person)
    {
        return await _uof.Transaction.Connection.QuerySingleAsync<uint>(@"
            INSERT `product` (`description`, `quantity`)
                       VALUE (@Description , @Quantity );
            SELECT CAST(LAST_INSERT_ID() AS UNSIGNED INTEGER);",
            person,
            _uof.Transaction
        );
    }

    public async Task<bool> UpdateAsync(uint id, ProductRequest person)
    {
        return await _uof.Transaction.Connection.ExecuteAsync(@"
            UPDATE `product` SET
                `description` = @Description,
                `quantity`    = @Quantity
            WHERE `id` = @id ;",
            new { id, person.Description, person.Quantity },
            _uof.Transaction
        ) > 0;
    }

    public async Task<bool> DeleteAsync(uint id)
    {
        return await _uof.Transaction.Connection.ExecuteAsync(@"
            DELETE FROM `product`
                WHERE `id` = @id ;",
            new { id },
            _uof.Transaction
        ) > 0;
    }
}