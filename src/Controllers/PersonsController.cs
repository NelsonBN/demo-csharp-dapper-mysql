using Microsoft.AspNetCore.Mvc;
using WebAPI.Dapper.Mysql.DTOs.Persons;
using WebAPI.Dapper.Mysql.Repositories;

namespace WebAPI.Dapper.Mysql.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonsController : ControllerBase
{
    private readonly PersonsRepository _repository;

    public PersonsController(
        PersonsRepository repository
    )
    {
        _repository = repository;
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _repository.GetAsync());
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] uint id)
    {
        var response = await _repository.GetAsync(id);
        return response is null ? NotFound() : Ok(response);
    }


    [HttpPost]
    public async Task<IActionResult> Add(PersonRequest request)
    {
        if(request.Validate() is string message)
        {
            return BadRequest(message);
        }

        var response = await _repository.AddAsync(request);
        return Ok(response);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] uint id, PersonRequest request)
    {
        if (request.Validate() is string message)
        {
            return BadRequest(message);
        }

        var response = await _repository.UpdateAsync(id, request);
        return response ? Ok() : NotFound();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] uint id)
    {
        var response = await _repository.DeleteAsync(id);
        return response ? NoContent() : NotFound();
    }
}