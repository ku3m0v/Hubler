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

    [HttpGet("detail/{roleName}")]
    public ActionResult<LkRole> Details(string roleName)
    {
        var role = _lkRoleDal.GetByRoleName(roleName);
        if (role == null)
        {
            return NotFound();
        }
        return Ok(role);
    }

    [HttpPost("insert")]
    public IActionResult CreateRole([FromBody] LkRole role)
    {
        _lkRoleDal.Insert(role);
        return Ok();
    }

    [HttpPost("edit")]
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

    [HttpGet]
    public ActionResult<IEnumerable<LkRole>> GetAllRoles()
    {
        var roles = _lkRoleDal.GetAll();
        return Ok(roles);
    }
}