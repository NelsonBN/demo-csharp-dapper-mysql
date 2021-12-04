using Microsoft.AspNetCore.Mvc;
using WebAPI.Dapper.Mysql.DTOs.Countries;
using WebAPI.Dapper.Mysql.Repositories;

namespace WebAPI.Dapper.Mysql.Controllers;

[ApiController]
[Route("[controller]")]
public class CountriesController : ControllerBase
{
    private readonly ILogger<CountriesController> _logger;
    private readonly CountriesRepository _repository;

    public CountriesController(
        ILogger<CountriesController> logger,
        CountriesRepository repository
    )
    {
        _logger = logger;
        _repository = repository;
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("Get country list");

        return Ok(await _repository.GetAllAsync());
    }

    [HttpGet("{iso2}")]
    public async Task<IActionResult> Get([FromRoute] string iso2)
    {
        _logger.LogInformation($"Get country {iso2}");

        return Ok(await _repository.GetAsync(iso2));
    }


    [HttpGet("pagination")]
    public async Task<IActionResult> Get([FromQuery] Pagination<CountryDTO> pagination)
    {
        _logger.LogInformation($"Get country list > paginated {pagination}");

        if (pagination.Validate())
        {
            return Ok(
                await _repository.GetAsync(pagination)
            );
        }

        return BadRequest();
    }


    [HttpGet("continents")]
    public async Task<IActionResult> GetWithContinents()
    {
        _logger.LogInformation("Get country list > with countries");

        return Ok(await _repository.GetFullAsync());
    }


    [HttpGet("search")]
    public async Task<IActionResult> GetDetails([FromQuery] string search)
    {
        _logger.LogInformation("Get country list > details");

        return Ok(await _repository.SearchAsync(search));
    }
}