using Microsoft.AspNetCore.Mvc;
using WebAPI.Dapper.Mysql.Repositories;

namespace WebAPI.Dapper.Mysql.Controllers;

[ApiController]
[Route("[controller]")]
public class EventSourcingController : ControllerBase
{
    private readonly EventSourcingRepository _repository;

    public EventSourcingController(
        EventSourcingRepository repository
    )
    {
        _repository = repository;
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _repository.GetAsync());
    }
}