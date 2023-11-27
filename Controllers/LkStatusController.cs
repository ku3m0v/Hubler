using Microsoft.AspNetCore.Mvc;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;

namespace Hubler.API.Controllers;

[ApiController]
[Route("api/status")]
public class LkStatusController : ControllerBase
{
    private readonly ILkStatusDAL _lkStatusDal;

    public LkStatusController(ILkStatusDAL lkStatusDal)
    {
        _lkStatusDal = lkStatusDal;
    }
    
    [HttpGet("detail/{statusName}")]
    public ActionResult<LkRole> Details(string statusName)
    {
        var status = _lkStatusDal.GetByName(statusName);
        if (status == null)
        {
            return NotFound();
        }
        return Ok(status);
    }

    [HttpGet]
    public ActionResult<IEnumerable<LkStatus>> GetAll()
    {
        var statuses = _lkStatusDal.GetAll();
        if (!statuses.Any())
        {
            return NotFound("No statuses found.");
        }
        return Ok(statuses);
    }

    [HttpPost]
    public IActionResult Insert([FromBody] LkStatus status)
    {
        if (status == null)
        {
            return BadRequest("Status is null.");
        }
        _lkStatusDal.Insert(status);
        return Ok("Status inserted successfully.");
    }

    [HttpPut]
    public IActionResult Update([FromBody] LkStatus status)
    {
        if (status == null)
        {
            return BadRequest("Status is null.");
        }
        _lkStatusDal.Update(status);
        return Ok("Status updated successfully.");
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var existingStatus = _lkStatusDal.GetById(id);
        if (existingStatus == null)
        {
            return NotFound($"Status with ID {id} not found.");
        }
        _lkStatusDal.Delete(id);
        return Ok("Status deleted successfully.");
    }
}