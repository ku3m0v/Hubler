using System.Security.Claims;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Hubler.Models;
using Hubler.ProductManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hubler.Controllers;

[Route("api/product_order")]
[ApiController]
public class ProductOrderController : ControllerBase
{
    private readonly ISupermarketDAL _supermarketDAL;
    private readonly IProductOrderDAL _productOrderDAL;
    private readonly IProductDAL _productDAL;
    private readonly IEmployeeDAL _employeeDAL;
    private readonly ILkProductDAL _lkProductDAL;
    private readonly IPerishableDAL _perishableDAL;
    private readonly INonPerishableDAL _nonPerishableDAL;
    private readonly IWarehouseDAL _warehouseDAL;
    
    public ProductOrderController(ISupermarketDAL supermarketDAL, 
        IProductOrderDAL productOrderDAL, 
        IProductDAL productDAL, 
        IEmployeeDAL employeeDAL, 
        ILkProductDAL lkProductDAL,
        IPerishableDAL perishableDAL,
        INonPerishableDAL nonPerishableDAL,
        IWarehouseDAL warehouseDAL)
    {
        _supermarketDAL = supermarketDAL;
        _productOrderDAL = productOrderDAL;
        _productDAL = productDAL;
        _employeeDAL = employeeDAL;
        _lkProductDAL = lkProductDAL;
        _perishableDAL = perishableDAL;
        _nonPerishableDAL = nonPerishableDAL;
        _warehouseDAL = warehouseDAL;
    }

    [HttpGet("list"), Authorize]
    public ActionResult<List<ProductOrderModel>> GetAll()
    {
        var productOrders = _productOrderDAL.GetAll();
        var productOrderModels = new List<ProductOrderModel>();

        int id = int.Parse(this.User.Claims.First(i => i.Type.Equals(ClaimTypes.NameIdentifier)).Value);
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (role == "admin")
        {
            foreach (var productOrder in productOrders)
            {
                var product = _lkProductDAL.GetById(productOrder.ProductId);
                var supermarket = _supermarketDAL.GetById(productOrder.SupermarketId);
                if (product != null && supermarket != null)
                {
                    productOrderModels.Add(new ProductOrderModel
                    {
                        Id = productOrder.Id,
                        SupermarketName = supermarket.Title,
                        ProductName = product.Title,
                        Quantity = productOrder.OrderedQuantity,
                        OrderDate = productOrder.OrderDate
                    });
                }
            }
        }
        else if (role == "manager")
        {
            var managerSupermarket = _supermarketDAL.GetById(id);
            foreach (var productOrder in productOrders)
            {
                var product = _lkProductDAL.GetById(productOrder.ProductId);
                var supermarket = _supermarketDAL.GetById(productOrder.SupermarketId);
                if (product != null && supermarket != null && supermarket.Id == managerSupermarket.Id)
                {
                    productOrderModels.Add(new ProductOrderModel
                    {
                        Id = productOrder.Id,
                        SupermarketName = supermarket.Title,
                        ProductName = product.Title,
                        Quantity = productOrder.OrderedQuantity,
                        OrderDate = productOrder.OrderDate
                    });
                }
            }
        }
        else
        {
            BadRequest("You don't have permission to view this page");
        }

        return Ok(productOrderModels);
    }
    
    [HttpPost("insert/{type}"), Authorize]
    public void Post([FromBody] ProductOrderModel model, string type)
    {
        var userId = int.Parse(this.User.Claims.First(i => i.Type.Equals(ClaimTypes.NameIdentifier)).Value);
        var employee = _employeeDAL.GetById(userId);
        var managerSupermarket = _supermarketDAL.GetById(employee.SupermarketId);
        if(type == "perishable")
        {
            
            var product = _lkProductDAL.GetByTitle(model.ProductName);
            
            var newProductOrder = new ProductOrder
            {
                SupermarketId = managerSupermarket.Id,
                ProductId = product.LkProductId,
                OrderedQuantity = model.Quantity,
                OrderDate = DateTime.UtcNow
            };
            
            _productOrderDAL.Insert(newProductOrder);
            
            var newProduct = new Product
            {
                Title = product.Title,
                CurrentPrice = product.CurrentPrice,
                ProductType = "P"
            };
            
            var productId = _productDAL.Insert(newProduct);
            
            var newPerishableProduct = new Perishable
            {
                ProductId = productId,
                ExpiryDate = model.ExpireDate,
                StorageType = model.StorageType
            };
            
            _perishableDAL.Insert(newPerishableProduct);
            
            var newWarehouse = new Warehouse
            {
                SupermarketId = managerSupermarket.Id,
                ProductId = productId,
                Quantity = model.Quantity
            };
            
            _warehouseDAL.Insert(newWarehouse);
        }
        else if(type == "nonperishable")
        {
            var product = _lkProductDAL.GetByTitle(model.ProductName);
            
            var newProductOrder = new ProductOrder
            {
                SupermarketId = managerSupermarket.Id,
                ProductId = product.LkProductId,
                OrderedQuantity = model.Quantity,
                OrderDate = DateTime.UtcNow
            };
            
            _productOrderDAL.Insert(newProductOrder);
            
            var newProduct = new Product
            {
                Title = product.Title,
                CurrentPrice = product.CurrentPrice,
                ProductType = "N"
            };
            
            var productId = _productDAL.Insert(newProduct);
            
            var newNonPerishableProduct = new NonPerishable
            {
                ProductId = productId,
                ShelfLife = model.ShelfLife
            };
            
            _nonPerishableDAL.Insert(newNonPerishableProduct);
            
            var newWarehouse = new Warehouse
            {
                SupermarketId = managerSupermarket.Id,
                ProductId = productId,
                Quantity = model.Quantity
            };
            
            _warehouseDAL.Insert(newWarehouse);
        }
        else
        {
            BadRequest("Invalid product type");
        }
    }
    
    [HttpDelete("delete")]
    public void Delete(int id)
    {
        _productOrderDAL.Delete(id);
    }
    
    [HttpGet("products")]
    public ActionResult getProducts()
    {
        var products = _lkProductDAL.GetAll();
        if(products == null)
        {
            return NotFound();
        }
        return Ok(products);
    }
}