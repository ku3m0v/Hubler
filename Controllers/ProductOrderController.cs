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
    private readonly ProductManager.ProductManager _productManager = new();
    
    public ProductOrderController(ISupermarketDAL supermarketDAL, IProductOrderDAL productOrderDAL, IProductDAL productDAL, IEmployeeDAL employeeDAL)
    {
        _supermarketDAL = supermarketDAL;
        _productOrderDAL = productOrderDAL;
        _productDAL = productDAL;
        _employeeDAL = employeeDAL;
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
                var product = _productDAL.GetById(productOrder.ProductId);
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
                var product = _productDAL.GetById(productOrder.ProductId);
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
            var products = _productManager.GetPerishableProducts();
            foreach (var product in products)
            {
                if (product.Title == model.ProductName)
                {
                    var newProduct = new Product
                    {
                        Title = product.Title,
                        CurrentPrice = product.Price,
                        ProductType = "P"
                    };
                    var productId = _productDAL.Insert(newProduct);
                    
                    var productOrder = new ProductOrder
                    {
                        SupermarketId = managerSupermarket.Id,
                        ProductId = productId,
                        OrderedQuantity = model.Quantity,
                        OrderDate = DateTime.UtcNow
                    };
                    _productOrderDAL.Insert(productOrder);
                }
            }
        }
        else if(type == "nonperishable")
        {
            var products = _productManager.GetNonPerishableProducts();
            foreach (var product in products)
            {
                if (product.Title == model.ProductName)
                {
                    var newProduct = new Product
                    {
                        Title = product.Title,
                        CurrentPrice = product.Price,
                        ProductType = "NP"
                    };
                    var productId = _productDAL.Insert(newProduct);
                    
                    var productOrder = new ProductOrder
                    {
                        SupermarketId = managerSupermarket.Id,
                        ProductId = productId,
                        OrderedQuantity = model.Quantity,
                        OrderDate = DateTime.UtcNow
                    };
                    _productOrderDAL.Insert(productOrder);
                }
            }
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
    
    [HttpGet("list/{type}")]
    public ActionResult getProducts(string type)
    {
        if (type == "perishable")
        {
            var products = _productManager.GetPerishableProducts();
            return Ok(products);
        }
        else if (type == "nonperishable")
        {
            var products = _productManager.GetNonPerishableProducts();
            return Ok(products);
        }
        else
        {
            return BadRequest("Invalid product type");
        }
    }
}