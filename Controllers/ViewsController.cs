using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hubler.Controllers;

[ApiController]
[Route("api/view")]
public class ViewsController : ControllerBase
{
    private readonly IViewDAL<ExpiredInventory> _expiredInventoryDal;
    private readonly IViewDAL<ExpiredWarehouse> _expiredWarehouseDal;
    private readonly IViewDAL<Top5ProductsBySupermarket> _top5ProductsBySupermarketDal;
    private readonly IViewDAL<SupermarketSalesSummary> _supermarketSalesSummaryDal;
    private readonly IEmployeeDAL _employeeDal;
    private readonly ISupermarketDAL _supermarketDal;

    public ViewsController(
        IViewDAL<ExpiredInventory> expiredInventoryDal,
        IViewDAL<ExpiredWarehouse> expiredWarehouseDal,
        IViewDAL<Top5ProductsBySupermarket> top5ProductsBySupermarketDal,
        IViewDAL<SupermarketSalesSummary> supermarketSalesSummaryDal,
        IEmployeeDAL employeeDal,
        ISupermarketDAL supermarketDal)
    {
        _expiredInventoryDal = expiredInventoryDal;
        _expiredWarehouseDal = expiredWarehouseDal;
        _top5ProductsBySupermarketDal = top5ProductsBySupermarketDal;
        _supermarketSalesSummaryDal = supermarketSalesSummaryDal;
        _employeeDal = employeeDal;
        _supermarketDal = supermarketDal;
    }

    // GET: api/view/ExpiredInventory
    [HttpGet("ExpiredInventory/{supermarketTitle}"), Authorize]
    public ActionResult<IEnumerable<ExpiredInventory>> GetExpiredInventory(string supermarketTitle)
    {
        // var id = int.Parse(this.User.Claims.First(i => i.Type.Equals(ClaimTypes.NameIdentifier)).Value);
        // var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        //
        // IEnumerable<ExpiredInventory> expiredInventories = _expiredInventoryDal.GetAll();
        //
        // switch (role)
        // {
        //     case "admin":
        //         return Ok(expiredInventories);
        //     case "manager":
        //     {
        //         var manager = _employeeDal.GetById(id);
        //         expiredInventories = expiredInventories.Where(i => i.SupermarketId == manager.SupermarketId);
        //         return Ok(expiredInventories);
        //     }
        //     default:
        //         return BadRequest("You don't have permission to view this page.");
        // }
        
        IEnumerable<ExpiredInventory> expiredInventories = _expiredInventoryDal.GetAll();
        expiredInventories = expiredInventories.Where(i => i.Supermarket_Title == supermarketTitle);
        return Ok(expiredInventories);
    }

    // GET: api/view/ExpiredWarehouse
    [HttpGet("ExpiredWarehouse/{supermarketTitle}"), Authorize]
    public ActionResult<IEnumerable<ExpiredWarehouse>> GetExpiredWarehouse(string supermarketTitle)
    {
        // var id = int.Parse(this.User.Claims.First(i => i.Type.Equals(ClaimTypes.NameIdentifier)).Value);
        // var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        //
        // IEnumerable<ExpiredWarehouse> expiredWarehouses = _expiredWarehouseDal.GetAll();
        //
        // switch (role)
        // {
        //     case "admin":
        //         return Ok(expiredWarehouses);
        //     case "manager":
        //     {
        //         var manager = _employeeDal.GetById(id);
        //         expiredWarehouses = expiredWarehouses.Where(i => i.SupermarketId == manager.SupermarketId);
        //         return Ok(expiredWarehouses);
        //     }
        //     default:
        //         return BadRequest("You don't have permission to view this page.");
        // }
        
        IEnumerable<ExpiredWarehouse> expiredWarehouses = _expiredWarehouseDal.GetAll();
        expiredWarehouses = expiredWarehouses.Where(i => i.Supermarket_Title == supermarketTitle);
        return Ok(expiredWarehouses);
    }

    // GET: api/view/Top5ProductsBySupermarket
    [HttpGet("Top5ProductsBySupermarket/{supermarketTitle}"), Authorize]
    public ActionResult<IEnumerable<Top5ProductsBySupermarket>> GetTop5ProductsBySupermarket(string supermarketTitle)
    {
        // var id = int.Parse(this.User.Claims.First(i => i.Type.Equals(ClaimTypes.NameIdentifier)).Value);
        // var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        //
        // IEnumerable<Top5ProductsBySupermarket> top5ProductsBySupermarkets = _top5ProductsBySupermarketDal.GetAll();
        //
        // switch (role)
        // {
        //     case "admin":
        //         return Ok(top5ProductsBySupermarkets);
        //     case "manager":
        //     {
        //         var manager = _employeeDal.GetById(id);
        //         var supermarket = _supermarketDal.GetById(manager.SupermarketId);
        //         top5ProductsBySupermarkets = top5ProductsBySupermarkets.Where(i => i.Supermarket_Name == supermarket.Title);
        //         return Ok(top5ProductsBySupermarkets);
        //     }
        //     default:
        //         return BadRequest("You don't have permission to view this page.");
        // }
        
        IEnumerable<Top5ProductsBySupermarket> top5ProductsBySupermarkets = _top5ProductsBySupermarketDal.GetAll();
        top5ProductsBySupermarkets = top5ProductsBySupermarkets.Where(i => i.Supermarket_Name == supermarketTitle);
        return Ok(top5ProductsBySupermarkets);
    }

    // GET: api/view/SupermarketSalesSummary
    [HttpGet("SupermarketSalesSummary/{supermarketTitle}"), Authorize]
    public ActionResult<IEnumerable<SupermarketSalesSummary>> GetSupermarketSalesSummary(string supermarketTitle)
    {
        // var id = int.Parse(this.User.Claims.First(i => i.Type.Equals(ClaimTypes.NameIdentifier)).Value);
        // var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        //
        // IEnumerable<SupermarketSalesSummary> supermarketSalesSummaries = _supermarketSalesSummaryDal.GetAll();
        //
        // switch (role)
        // {
        //     case "admin":
        //         return Ok(supermarketSalesSummaries);
        //     case "manager":
        //     {
        //         var manager = _employeeDal.GetById(id);
        //         supermarketSalesSummaries = supermarketSalesSummaries.Where(i => i.SupermarketId == manager.SupermarketId);
        //         return Ok(supermarketSalesSummaries);
        //     }
        //     default:
        //         return BadRequest("You don't have permission to view this page.");
        //}
        
        IEnumerable<SupermarketSalesSummary> supermarketSalesSummaries = _supermarketSalesSummaryDal.GetAll();
        var supermarket = _supermarketDal.GetSupermarketByTitle(supermarketTitle);
        supermarketSalesSummaries = supermarketSalesSummaries.Where(i => i.SupermarketId == supermarket.Id);
        return Ok(supermarketSalesSummaries);
    }
    
    [HttpGet("titles"), Authorize]
    public IActionResult GetSupermarketTitles()
    {
        var id = int.Parse(this.User.Claims.First(i => i.Type.Equals(ClaimTypes.NameIdentifier)).Value);
        var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        
        var supermarketTitles = _supermarketDal.GetAllTitles();


        switch (role)
        {
            case "admin":
                return Ok(supermarketTitles);
            case "manager":
            {
                var manager = _employeeDal.GetById(id);
                var supermarket = _supermarketDal.GetById(manager.SupermarketId);
                supermarketTitles = supermarketTitles.Where(i => i == supermarket.Title);
                return Ok(supermarketTitles);
            }
            default:
                return BadRequest("You don't have permission to view this page.");
        }
    }
}