using Microsoft.AspNetCore.Mvc;
using WebAPI.Dapper.Mysql.DTOs.Products;
using WebAPI.Dapper.Mysql.Helpers;
using WebAPI.Dapper.Mysql.Repositories;

namespace WebAPI.Dapper.Mysql.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private static uint TEST_COUNT = 0;

    private readonly IUnitOfWork _uof;
    private readonly ProductsRepository _productsRrepository;
    private readonly EventSourcingRepository _eventSourcingRepository;

    public ProductsController(
        IUnitOfWork uof,
        ProductsRepository productsRrepository,
        EventSourcingRepository eventSourcingRepository
    )
    {
        _uof = uof;
        _productsRrepository = productsRrepository;
        _eventSourcingRepository = eventSourcingRepository;
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _productsRrepository.GetAsync());
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] uint id)
    {
        var response = await _productsRrepository.GetAsync(id);
        return response is null ? NotFound() : Ok(response);
    }


    [HttpPost]
    public async Task<IActionResult> Add(ProductRequest request)
    {
        TEST_COUNT++;

        if (request.Validate() is string message)
        {
            return BadRequest(message);
        }

        var response = await _productsRrepository.AddAsync(request);
        _eventSourcingRepository?.AddAsync($"Added product > Id: {response} - {request.Description} - Q:{request.Quantity}");

        if(TEST_COUNT % 3 == 0)
        {
            _uof.Rollback();

            throw new Exception("Exception to test transaction of the UoW");
        }

        _uof.Commit();

        return Ok(response);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] uint id, ProductRequest request)
    {
        if (request.Validate() is string message)
        {
            return BadRequest(message);
        }

        var response = await _productsRrepository.UpdateAsync(id, request);

        if (response)
        {
            _eventSourcingRepository?.AddAsync($"Updated product > Id: {id} - {request.Description} - Q:{request.Quantity}");
            _uof.Commit();

            return Ok();
        }

        return NotFound();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] uint id)
    {
        var response = await _productsRrepository.DeleteAsync(id);

        if (response)
        {
            _eventSourcingRepository?.AddAsync($"Delted product > Id: {id}");
            _uof.Commit();

            return NoContent();
        }

        return NotFound();
    }
}