using AdoNetDapper.Models;
using AdoNetDapper.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AdoNetDapper.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserAdoNetRepository _repository = new();

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_repository.Get());
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var user = _repository.Get(id);

        if (user == null)
        {
            return NotFound(); //ERRO HTTP: 404 - Not Found
        }

        return Ok(user);
    }

    [HttpPost]
    public IActionResult Insert([FromBody] User user)
    {
        //try
        //{
        _repository.Insert(user);
        return Ok(user);
        //}
        //catch (Exception e)
        //{
        //    return StatusCode(500, e.Message);
        //}
    }

    [HttpPut]
    public IActionResult Update([FromBody] User user)
    {
        try
        {
            _repository.Update(user);
            return Ok(user);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _repository.Delete(id);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
