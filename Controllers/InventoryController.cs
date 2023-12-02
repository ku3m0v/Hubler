using System.Security.Claims;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Hubler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hubler.Controllers;

[Route("api/inventory")]
[ApiController]
public class InventoryController : ControllerBase
{
    private readonly IInventoryDAL _inventoryDal;
    private readonly ISupermarketDAL _supermarketDal;
    private readonly IProductDAL _productDal;
    private readonly IPerishableDAL _perishableDal;
    private readonly INonPerishableDAL _nonPerishableDal;
    private readonly IEmployeeDAL _employeeDal;

    public InventoryController(IInventoryDAL inventoryDal,
        ISupermarketDAL supermarketDal,
        IProductDAL productDal,
        IPerishableDAL perishableDal,
        INonPerishableDAL nonPerishableDal,
        IEmployeeDAL employeeDal)
    {
        _inventoryDal = inventoryDal;
        _supermarketDal = supermarketDal;
        _productDal = productDal;
        _perishableDal = perishableDal;
        _nonPerishableDal = nonPerishableDal;
        _employeeDal = employeeDal;
    }

    [HttpGet("list"), Authorize]
    public ActionResult<List<InventoryModel>> GetAll()
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        switch (role)
        {
            case "admin":
            {
                var inventories = _inventoryDal.GetAll();
                var inventoryModels = new List<InventoryModel>();

                foreach (var inventory in inventories)
                {
                    var supermarket = _supermarketDal.GetById(inventory.SupermarketId);
                    var product = _productDal.GetById(inventory.ProductId);

                    var inventoryModel = new InventoryModel
                    {
                        Id = inventory.Id,
                        SupermarketTitle = supermarket?.Title,
                        ProductId = product?.Id ?? 0,
                        Quantity = inventory.Quantity,
                        Title = product?.Title,
                        CurrentPrice = product?.CurrentPrice ?? 0,
                        ProductType = product?.ProductType
                    };

                    if (product?.ProductType == "P")
                    {
                        var perishable = _perishableDal.GetByProductId(inventory.ProductId);
                        inventoryModel.ExpiryDate = perishable.ExpiryDate;
                        inventoryModel.StorageType = perishable.StorageType;
                    }
                    else
                    {
                        var nonPerishable = _nonPerishableDal.GetByProductId(inventory.ProductId);
                        inventoryModel.ShelfLife = nonPerishable.ShelfLife;
                    }

                    inventoryModels.Add(inventoryModel);
                }

                return Ok(inventoryModels);
            }
            case "manager":
            {
                var id = int.Parse(this.User.Claims.First(i => i.Type.Equals(ClaimTypes.NameIdentifier)).Value);
                
                var employee = _employeeDal.GetById(id);
                var managerSupermarket = _supermarketDal.GetById(employee.SupermarketId);
                var inventories = _inventoryDal.GetAll().Where(w => w.SupermarketId == managerSupermarket.Id);
                var inventoryModels = new List<InventoryModel>();

                foreach (var inventory in inventories)
                {
                    var product = _productDal.GetById(inventory.ProductId);

                    var inventoryModel = new InventoryModel
                    {
                        Id = inventory.Id,
                        SupermarketTitle = managerSupermarket?.Title,
                        ProductId = product?.Id ?? 0,
                        Quantity = inventory.Quantity,
                        Title = product?.Title,
                        CurrentPrice = product?.CurrentPrice ?? 0,
                        ProductType = product?.ProductType
                    };

                    if (product?.ProductType == "P")
                    {
                        var perishable = _perishableDal.GetByProductId(inventory.ProductId);
                        inventoryModel.ExpiryDate = perishable.ExpiryDate;
                        inventoryModel.StorageType = perishable.StorageType;
                    }
                    else
                    {
                        var nonPerishable = _nonPerishableDal.GetByProductId(inventory.ProductId);
                        inventoryModel.ShelfLife = nonPerishable.ShelfLife;
                    }

                    inventoryModels.Add(inventoryModel);
                }

                return Ok(inventoryModels);
            }
            default:
                return BadRequest("You don't have permission to view this page.");
        }
    }

    // [HttpPost("insert")]
    // public void Insert(InventoryModel inventoryModel)
    // {
    //     if (inventoryModel.ProductType == "P")
    //     {
    //         var product = new Product
    //         {
    //             Title = inventoryModel.Title,
    //             CurrentPrice = inventoryModel.CurrentPrice,
    //             ProductType = inventoryModel.ProductType
    //         };
    //         var productId = _productDal.Insert(product);
    //         var perishable = new Perishable
    //         {
    //             ExpiryDate = inventoryModel.ExpiryDate,
    //             StorageType = inventoryModel.StorageType,
    //             ProductId = productId
    //         };
    //         _perishableDal.Insert(perishable);
    //         var inventory = new Inventory
    //         {
    //             SupermarketId = _supermarketDal.GetSupermarketByTitle(inventoryModel.SupermarketTitle).Id,
    //             ProductId = productId,
    //             Quantity = inventoryModel.Quantity
    //         };
    //         _inventoryDal.Insert(inventory);
    //     }
    //     else
    //     {
    //         var product = new Product
    //         {
    //             Title = inventoryModel.Title,
    //             CurrentPrice = inventoryModel.CurrentPrice,
    //             ProductType = inventoryModel.ProductType
    //         };
    //         var productId = _productDal.Insert(product);
    //         var nonPerishable = new NonPerishable
    //         {
    //             ShelfLife = inventoryModel.ShelfLife,
    //             ProductId = productId
    //         };
    //         _nonPerishableDal.Insert(nonPerishable);
    //         var inventory = new Inventory
    //         {
    //             SupermarketId = _supermarketDal.GetSupermarketByTitle(inventoryModel.SupermarketTitle).Id,
    //             ProductId = productId,
    //             Quantity = inventoryModel.Quantity
    //         };
    //         _inventoryDal.Insert(inventory);
    //     }
    // }
    //
    // [HttpGet("get")]
    // public ActionResult<InventoryModel> GetById(int id)
    // {
    //     var inventory = _inventoryDal.GetById(id);
    //
    //     if (inventory == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     var supermarket = _supermarketDal.GetById(inventory.SupermarketId);
    //     var product = _productDal.GetById(inventory.ProductId);
    //
    //     var inventoryModel = new InventoryModel
    //     {
    //         Id = inventory.Id,
    //         SupermarketTitle = supermarket?.Title,
    //         ProductId = product?.Id ?? 0,
    //         Quantity = inventory.Quantity,
    //         Title = product?.Title,
    //         CurrentPrice = product?.CurrentPrice ?? 0,
    //         ProductType = product?.ProductType
    //     };
    //
    //     if (product?.ProductType == "P")
    //     {
    //         var perishable = _perishableDal.GetByProductId(inventory.ProductId);
    //         inventoryModel.ExpiryDate = perishable.ExpiryDate;
    //         inventoryModel.StorageType = perishable.StorageType;
    //     }
    //     else
    //     {
    //         var nonPerishable = _nonPerishableDal.GetByProductId(inventory.ProductId);
    //         inventoryModel.ShelfLife = nonPerishable.ShelfLife;
    //     }
    //
    //     return Ok(inventoryModel);
    // }
    //
    // [HttpPost("update")]
    // public IActionResult Update([FromBody] InventoryModel model)
    // {
    //     var inventory = _inventoryDal.GetById(model.Id);
    //     if (inventory == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     var product = _productDal.GetById(inventory.ProductId);
    //     if (product == null)
    //     {
    //         return NotFound("Product not found.");
    //     }
    //
    //     var updatedProduct = new Product
    //     {
    //         Id = product.Id,
    //         Title = model.Title,
    //         CurrentPrice = model.CurrentPrice,
    //         ProductType = model.ProductType
    //     };
    //
    //     _productDal.Update(updatedProduct);
    //
    //     if (model.ProductType == "P")
    //     {
    //         var perishable = _perishableDal.GetByProductId(inventory.ProductId);
    //         if (perishable == null)
    //         {
    //             return NotFound("Perishable not found.");
    //         }
    //
    //         var updatedPerishable = new Perishable
    //         {
    //             ExpiryDate = model.ExpiryDate,
    //             StorageType = model.StorageType,
    //             ProductId = perishable.ProductId
    //         };
    //
    //         _perishableDal.Update(updatedPerishable);
    //     }
    //     else
    //     {
    //         var nonPerishable = _nonPerishableDal.GetByProductId(inventory.ProductId);
    //         if (nonPerishable == null)
    //         {
    //             return NotFound("Non perishable not found.");
    //         }
    //
    //         var updatedNonPerishable = new NonPerishable
    //         {
    //             ShelfLife = model.ShelfLife,
    //             ProductId = nonPerishable.ProductId
    //         };
    //
    //         _nonPerishableDal.Update(updatedNonPerishable);
    //     }
    //
    //     inventory.Quantity = model.Quantity;
    //     _inventoryDal.Update(inventory);
    //
    //     return Ok();
    // }
    
    [HttpDelete("delete")]
    public void Delete(int id)
    {
        _inventoryDal.Delete(id);
    }
}