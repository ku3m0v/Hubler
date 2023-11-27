using Microsoft.AspNetCore.Mvc;
using Hubler.DAL.Interfaces;
using Hubler.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Hubler.DAL.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hubler.Controllers;

[ApiController]
[Route("api/sale")]
public class SaleController : ControllerBase
{
    private readonly ISaleDAL _saleDal;
    private readonly ISaleDetailDAL _saleDetailDal;
    private readonly IProductDAL _productDal;
    private readonly ISupermarketDAL _supermarketDal;
    private readonly IEmployeeDAL _employeeDal;

    public SaleController(ISaleDAL saleDal,
        ISaleDetailDAL saleDetailDal,
        IProductDAL productDal,
        ISupermarketDAL supermarketDal,
        IEmployeeDAL employeeDal)
    {
        _saleDal = saleDal;
        _saleDetailDal = saleDetailDal;
        _productDal = productDal;
        _supermarketDal = supermarketDal;
        _employeeDal = employeeDal;
    }
    
    [HttpGet("list"), Authorize]
    public ActionResult<List<SaleModel>> GetAll()
    {
        var sales = _saleDal.GetAll();
        var saleDetails = _saleDetailDal.GetAll();
        var saleModels = new List<SaleModel>();
        
        int id = int.Parse(this.User.Claims.First(i => i.Type.Equals(ClaimTypes.NameIdentifier)).Value);
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (role == "admin")
        {
            foreach (var sale in sales)
            {
                var supermarket = _supermarketDal.GetById(sale.SupermarketId);
                var saleDetailsBySale = saleDetails.Where(sd => sd.SaleId == sale.Id).ToList();
                foreach (var saleDetail in saleDetailsBySale)
                {
                    var product = _productDal.GetById(saleDetail.ProductId);
                    saleModels.Add(new SaleModel
                    {
                        SaleId = sale.Id,
                        SupermarketName = supermarket.Title,
                        SaleDate = sale.DateAndTime,
                        SaleDetailId = saleDetail.Id,
                        ProductName = product.Title,
                        QuantitySold = saleDetail.QuantitySold,
                        TotalPrice = saleDetail.TotalPrice
                    });
                }
                
            }
        }
        else if(role == "manager")
        {
            var employee = _employeeDal.GetById(id);
            var managerSupermarket = _supermarketDal.GetById(employee.SupermarketId);
            var managerSales = sales.Where(s => s.SupermarketId == managerSupermarket.Id);
            foreach (var sale in managerSales)
            {
                var saleDetailsBySale = saleDetails.Where(sd => sd.SaleId == sale.Id).ToList();
                foreach (var saleDetail in saleDetailsBySale)
                {
                    var product = _productDal.GetById(saleDetail.ProductId);
                    saleModels.Add(new SaleModel
                    {
                        SaleId = sale.Id,
                        SupermarketName = managerSupermarket.Title,
                        SaleDate = sale.DateAndTime,
                        SaleDetailId = saleDetail.Id,
                        ProductName = product.Title,
                        QuantitySold = saleDetail.QuantitySold,
                        TotalPrice = saleDetail.TotalPrice
                    });
                }
            }
        }
        else
        {
            return BadRequest("You don't have permission to view this page.");
        }

        return Ok(saleModels);
    }
    
    [HttpPost("insert"), Authorize]
    public ActionResult Insert(SaleModel saleModel)
    {
        var managerId = int.Parse(this.User.Claims.First(i => i.Type.Equals(ClaimTypes.NameIdentifier)).Value);
        var manager = _employeeDal.GetById(managerId);
        
        var sale = new Sale
        {
            SupermarketId = manager.SupermarketId,
            DateAndTime = saleModel.SaleDate
        };
        var saleId = _saleDal.Insert(sale);
        
        var saleDetail = new SaleDetail
        {
            SaleId = saleId,
            ProductId = saleModel.ProductId,
            QuantitySold = saleModel.QuantitySold,
            TotalPrice = saleModel.TotalPrice
        };
        _saleDetailDal.Insert(saleDetail);
        
        return Ok();
    }
    
    [HttpDelete("delete")]
    public ActionResult Delete(int id)
    {
        var saleDetails = _saleDetailDal.GetAll();
        var saleDetailsBySale = saleDetails.Where(sd => sd.SaleId == id).ToList();
        foreach (var saleDetail in saleDetailsBySale)
        {
            _saleDetailDal.Delete(saleDetail.Id);
        }
        _saleDal.Delete(id);
        return Ok();
    }
    
    [HttpGet("products"), Authorize]
    public ActionResult<List<Product>> GetProducts()
    {
        var id = int.Parse(this.User.Claims.First(i => i.Type.Equals(ClaimTypes.NameIdentifier)).Value);
        var employee = _employeeDal.GetById(id);
        var supermarket = _supermarketDal.GetById(employee.SupermarketId);
        
        var products = _productDal.GetProductsBySupermarket(supermarket.Id);
        return Ok(products);
    }
}