using Microsoft.AspNetCore.Mvc;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;

namespace Hubler.Controllers;

[ApiController]
[Route("api/view")]
public class ViewsController : ControllerBase
{
    private readonly IViewDAL<ExpiredInventory> _expiredInventoryDal;
    private readonly IViewDAL<ExpiredWarehouse> _expiredWarehouseDal;
    private readonly IViewDAL<Top5ProductsBySupermarket> _top5ProductsBySupermarketDal;
    private readonly IViewDAL<SupermarketSalesSummary> _supermarketSalesSummaryDal;

    public ViewsController(
        IViewDAL<ExpiredInventory> expiredInventoryDal,
        IViewDAL<ExpiredWarehouse> expiredWarehouseDal,
        IViewDAL<Top5ProductsBySupermarket> top5ProductsBySupermarketDal,
        IViewDAL<SupermarketSalesSummary> supermarketSalesSummaryDal)
    {
        _expiredInventoryDal = expiredInventoryDal;
        _expiredWarehouseDal = expiredWarehouseDal;
        _top5ProductsBySupermarketDal = top5ProductsBySupermarketDal;
        _supermarketSalesSummaryDal = supermarketSalesSummaryDal;
    }

    // GET: api/view/ExpiredInventory
    [HttpGet("ExpiredInventory")]
    public ActionResult<IEnumerable<ExpiredInventory>> GetExpiredInventory()
    {
        return Ok(_expiredInventoryDal.GetAll());
    }

    // GET: api/view/ExpiredWarehouse
    [HttpGet("ExpiredWarehouse")]
    public ActionResult<IEnumerable<ExpiredWarehouse>> GetExpiredWarehouse()
    {
        return Ok(_expiredWarehouseDal.GetAll());
    }

    // GET: api/view/Top5ProductsBySupermarket
    [HttpGet("Top5ProductsBySupermarket")]
    public ActionResult<IEnumerable<Top5ProductsBySupermarket>> GetTop5ProductsBySupermarket()
    {
        return Ok(_top5ProductsBySupermarketDal.GetAll());
    }

    // GET: api/view/SupermarketSalesSummary
    [HttpGet("SupermarketSalesSummary")]
    public ActionResult<IEnumerable<SupermarketSalesSummary>> GetSupermarketSalesSummary()
    {
        return Ok(_supermarketSalesSummaryDal.GetAll());
    }
}