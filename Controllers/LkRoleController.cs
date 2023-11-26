using Microsoft.AspNetCore.Mvc;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;

namespace Hubler.API.Controllers;

[ApiController]
[Route("api/role")]
public class RolesController : ControllerBase
{
    private readonly ILkRoleDAL _lkRoleDal;

    public RolesController(ILkRoleDAL lkRoleDal)
    {
        _lkRoleDal = lkRoleDal;
    }

    [HttpPost("insert")]
    public IActionResult CreateRole([FromBody] LkRole role)
    {
        _lkRoleDal.Insert(role);
        return Ok();
    }

    [HttpPut("edit")]
    public IActionResult UpdateRole([FromBody] LkRole role)
    {
        _lkRoleDal.Update(role);
        return NoContent();
    }

    [HttpDelete]
    public IActionResult DeleteRole(int id)
    {
        var existingRole = _lkRoleDal.GetById(id);
        if (existingRole == null)
        {
            return NotFound();
        }
        _lkRoleDal.Delete(id);
        return NoContent();
    }

    [HttpGet("list")]
    public ActionResult<IEnumerable<LkRole>> GetAllRoles()
    {
        var roles = _lkRoleDal.GetAll();
        return Ok(roles);
    }
}