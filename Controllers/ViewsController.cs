using Microsoft.AspNetCore.Mvc;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;

namespace Hubler.Controllers;

[ApiController]
[Route("api/view")]
public class ViewsController : ControllerBase
{
    private readonly IViewDAL<ExpiredInventory> _expiredInventoryDAL;
    private readonly IViewDAL<ExpiredWarehouse> _expiredWarehouseDAL;
    private readonly IViewDAL<Top5ProductsBySupermarket> _top5ProductsBySupermarketDAL;
    private readonly IViewDAL<SupermarketSalesSummary> _supermarketSalesSummaryDAL;

    public ViewsController(
        IViewDAL<ExpiredInventory> expiredInventoryDAL,
        IViewDAL<ExpiredWarehouse> expiredWarehouseDAL,
        IViewDAL<Top5ProductsBySupermarket> top5ProductsBySupermarketDAL,
        IViewDAL<SupermarketSalesSummary> supermarketSalesSummaryDAL)
    {
        _expiredInventoryDAL = expiredInventoryDAL;
        _expiredWarehouseDAL = expiredWarehouseDAL;
        _top5ProductsBySupermarketDAL = top5ProductsBySupermarketDAL;
        _supermarketSalesSummaryDAL = supermarketSalesSummaryDAL;
    }

    // GET: api/view/ExpiredInventory
    [HttpGet("ExpiredInventory")]
    public ActionResult<IEnumerable<ExpiredInventory>> GetExpiredInventory()
    {
        return Ok(_expiredInventoryDAL.GetAll());
    }

    // GET: api/view/ExpiredWarehouse
    [HttpGet("ExpiredWarehouse")]
    public ActionResult<IEnumerable<ExpiredWarehouse>> GetExpiredWarehouse()
    {
        return Ok(_expiredWarehouseDAL.GetAll());
    }

    // GET: api/view/Top5ProductsBySupermarket
    [HttpGet("Top5ProductsBySupermarket")]
    public ActionResult<IEnumerable<Top5ProductsBySupermarket>> GetTop5ProductsBySupermarket()
    {
        return Ok(_top5ProductsBySupermarketDAL.GetAll());
    }

    // GET: api/view/SupermarketSalesSummary
    [HttpGet("SupermarketSalesSummary")]
    public ActionResult<IEnumerable<SupermarketSalesSummary>> GetSupermarketSalesSummary()
    {
        return Ok(_supermarketSalesSummaryDAL.GetAll());
    }
}