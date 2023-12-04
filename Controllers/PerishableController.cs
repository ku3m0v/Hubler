using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Hubler.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hubler.Controllers;

[ApiController]
[Route("api/perishable")]
public class PerishableController : ControllerBase
{
    
    private readonly IProductDAL _productDAL;
    private readonly IPerishableDAL _perishableDAL;
    
    
    public PerishableController(IProductDAL productDAL, IPerishableDAL perishableDAL)
    {
        _productDAL = productDAL;
        _perishableDAL = perishableDAL;
    }
    
    // GET: api/{type}/list
    [HttpGet("list"), Authorize]
    public IActionResult GetAll()
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (role == "admin" || role == "manager")
        {
            var products = _productDAL.GetAll();
            var perishableProducts = new List<PerishableProductModel>();

            foreach (var product in products)
            {
                if (product.ProductType == "P")
                {
                    var perishable = _perishableDAL.GetByProductId(product.Id);
                    perishableProducts.Add(new PerishableProductModel
                    {
                        ProductId = product.Id,
                        Title = product.Title,
                        CurrentPrice = product.CurrentPrice,
                        ProductType = product.ProductType,
                        ExpiryDate = perishable.ExpiryDate,
                        StorageType = perishable.StorageType
                    });
                }
            }

            return Ok(perishableProducts);
        }

        return BadRequest("You do not have permission to view this resource.");
    }
    
    // POST: api/{type}/insert
    [HttpPost("insert")]
    public IActionResult Insert(PerishableProductModel model)
    {
        if(model == null)
        {
            return BadRequest("Invalid model.");
        }
        
        var product = new Product
        {
            Title = model.Title,
            CurrentPrice = model.CurrentPrice,
            ProductType = model.ProductType
        };
        
        var productId = _productDAL.Insert(product);

        var perishable = new Perishable
        {
            ProductId = productId,
            ExpiryDate = model.ExpiryDate,
            StorageType = model.StorageType
        };
        _perishableDAL.Insert(perishable);

        return Ok();
    }
    
    // GET: api/{type}/detail
    [HttpGet("detail")]
    public IActionResult GetById(int id)
    {
        var product = _productDAL.GetById(id);

        if (product == null)
        {
            return NotFound();
        }

        if (product.ProductType == "P")
        {
            var perishable = _perishableDAL.GetByProductId(product.Id);
            return Ok(new PerishableProductModel
            {
                ProductId = product.Id,
                Title = product.Title,
                CurrentPrice = product.CurrentPrice,
                ProductType = product.ProductType,
                ExpiryDate = perishable.ExpiryDate,
                StorageType = perishable.StorageType
            });
        }
        
        return BadRequest("Id does not match a perishable product.");
    }
    
    // POST: api/{type}/update
    [HttpPost("update")]
    public IActionResult Update(PerishableProductModel model)
    {
        if(model == null)
        {
            return BadRequest("Invalid model.");
        }
        
        var product = new Product
        {
            Id = model.ProductId,
            Title = model.Title,
            CurrentPrice = model.CurrentPrice,
            ProductType = model.ProductType
        };
        
        _productDAL.Update(product);

        var perishable = new Perishable
        {
            ProductId = model.ProductId,
            ExpiryDate = model.ExpiryDate,
            StorageType = model.StorageType
        };
        _perishableDAL.Update(perishable);

        return Ok();
    }
    
    // DELETE: api/{type}/delete
    [HttpDelete("delete")]
    public IActionResult Delete(int id)
    {
        var product = _productDAL.GetById(id);

        if (product == null)
        {
            return NotFound();
        }

        if (product.ProductType == "P")
        {
            _perishableDAL.Delete(id);
            _productDAL.Delete(id);
            return Ok();
        }
        
        return BadRequest("Id does not match a perishable product.");
    }
    
}