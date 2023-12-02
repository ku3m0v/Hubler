using System.Security.Claims;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Hubler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hubler.Controllers;

[Route("api/warehouse")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseDAL _warehouseDal;
    private readonly ISupermarketDAL _supermarketDal;
    private readonly IProductDAL _productDal;
    private readonly IPerishableDAL _perishableDal;
    private readonly INonPerishableDAL _nonPerishableDal;
    private readonly IEmployeeDAL _employeeDal;

    public WarehouseController(IWarehouseDAL warehouseDal,
        ISupermarketDAL supermarketDal,
        IProductDAL productDal,
        IPerishableDAL perishableDal,
        INonPerishableDAL nonPerishableDal,
        IEmployeeDAL employeeDal)
    {
        _warehouseDal = warehouseDal;
        _supermarketDal = supermarketDal;
        _productDal = productDal;
        _perishableDal = perishableDal;
        _nonPerishableDal = nonPerishableDal;
        _employeeDal = employeeDal;
    }

    // GET: api/warehouse/list
    [HttpGet("list"), Authorize]
    public ActionResult<List<WarehouseModel>> GetAll()
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        switch (role)
        {
            case "admin":
            {
                var warehouses = _warehouseDal.GetAll();
                var warehouseModels = new List<WarehouseModel>();

                foreach (var warehouse in warehouses)
                {
                    var supermarket = _supermarketDal.GetById(warehouse.SupermarketId);
                    var product = _productDal.GetById(warehouse.ProductId);

                    var warehouseModel = new WarehouseModel
                    {
                        Id = warehouse.Id,
                        SupermarketTitle = supermarket?.Title,
                        ProductId = product?.Id ?? 0,
                        Quantity = warehouse.Quantity,
                        Title = product?.Title,
                        CurrentPrice = product?.CurrentPrice ?? 0,
                        ProductType = product?.ProductType
                    };

                    if (product?.ProductType == "P")
                    {
                        var perishable = _perishableDal.GetByProductId(warehouse.ProductId);
                        warehouseModel.ExpiryDate = perishable.ExpiryDate;
                        warehouseModel.StorageType = perishable.StorageType;
                    }
                    else
                    {
                        var nonPerishable = _nonPerishableDal.GetByProductId(warehouse.ProductId);
                        warehouseModel.ShelfLife = nonPerishable.ShelfLife;
                    }

                    warehouseModels.Add(warehouseModel);
                }

                return Ok(warehouseModels);
            }
            case "manager":
            {
                var id = int.Parse(this.User.Claims.First(i => i.Type.Equals(ClaimTypes.NameIdentifier)).Value);
                var employee = _employeeDal.GetById(id);
                var managerSupermarket = _supermarketDal.GetById(employee.SupermarketId);
                var warehouses = _warehouseDal.GetAll().Where(w => w.SupermarketId == managerSupermarket.Id);
                var warehouseModels = new List<WarehouseModel>();

                foreach (var warehouse in warehouses)
                {
                    var product = _productDal.GetById(warehouse.ProductId);

                    var warehouseModel = new WarehouseModel
                    {
                        Id = warehouse.Id,
                        SupermarketTitle = managerSupermarket?.Title,
                        ProductId = product?.Id ?? 0,
                        Quantity = warehouse.Quantity,
                        Title = product?.Title,
                        CurrentPrice = product?.CurrentPrice ?? 0,
                        ProductType = product?.ProductType
                    };

                    if (product?.ProductType == "P")
                    {
                        var perishable = _perishableDal.GetByProductId(warehouse.ProductId);
                        warehouseModel.ExpiryDate = perishable.ExpiryDate;
                        warehouseModel.StorageType = perishable.StorageType;
                    }
                    else
                    {
                        var nonPerishable = _nonPerishableDal.GetByProductId(warehouse.ProductId);
                        warehouseModel.ShelfLife = nonPerishable.ShelfLife;
                    }

                    warehouseModels.Add(warehouseModel);
                }

                return Ok(warehouseModels);
            }
            default:
                return BadRequest("You don't have permission to view this page.");
        }
    }

    // POST: api/warehouse/insert
    // [HttpPost("insert")]
    // public void Insert(WarehouseModel warehouseModel)
    // {
    //     if (warehouseModel.ProductType == "P")
    //     {
    //         var product = new Product
    //         {
    //             Title = warehouseModel.Title,
    //             CurrentPrice = warehouseModel.CurrentPrice,
    //             ProductType = warehouseModel.ProductType
    //         };
    //         var productId = _productDal.Insert(product);
    //         var perishable = new Perishable
    //         {
    //             ExpiryDate = warehouseModel.ExpiryDate,
    //             StorageType = warehouseModel.StorageType,
    //             ProductId = productId
    //         };
    //         _perishableDal.Insert(perishable);
    //         var warehouse = new Warehouse
    //         {
    //             SupermarketId = _supermarketDal.GetSupermarketByTitle(warehouseModel.SupermarketTitle).Id,
    //             ProductId = productId,
    //             Quantity = warehouseModel.Quantity
    //         };
    //         _warehouseDal.Insert(warehouse);
    //     }
    //     else
    //     {
    //         var product = new Product
    //         {
    //             Title = warehouseModel.Title,
    //             CurrentPrice = warehouseModel.CurrentPrice,
    //             ProductType = warehouseModel.ProductType
    //         };
    //         var productId = _productDal.Insert(product);
    //         var nonPerishable = new NonPerishable
    //         {
    //             ShelfLife = warehouseModel.ShelfLife,
    //             ProductId = productId
    //         };
    //         _nonPerishableDal.Insert(nonPerishable);
    //         var warehouse = new Warehouse
    //         {
    //             SupermarketId = _supermarketDal.GetSupermarketByTitle(warehouseModel.SupermarketTitle).Id,
    //             ProductId = productId,
    //             Quantity = warehouseModel.Quantity
    //         };
    //         _warehouseDal.Insert(warehouse);
    //     }
    // }
    //
    //
    // // GET: api/warehouse/get
    // [HttpGet("get")]
    // public ActionResult<WarehouseModel> GetById(int id)
    // {
    //     var warehouse = _warehouseDal.GetById(id);
    //
    //     if (warehouse == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     var supermarket = _supermarketDal.GetById(warehouse.SupermarketId);
    //     var product = _productDal.GetById(warehouse.ProductId);
    //
    //     var warehouseModel = new WarehouseModel
    //     {
    //         Id = warehouse.Id,
    //         SupermarketTitle = supermarket?.Title,
    //         ProductId = product?.Id ?? 0,
    //         Quantity = warehouse.Quantity,
    //         Title = product?.Title,
    //         CurrentPrice = product?.CurrentPrice ?? 0,
    //         ProductType = product?.ProductType
    //     };
    //
    //     if (product?.ProductType == "P")
    //     {
    //         var perishable = _perishableDal.GetByProductId(warehouse.ProductId);
    //         warehouseModel.ExpiryDate = perishable.ExpiryDate;
    //         warehouseModel.StorageType = perishable.StorageType;
    //     }
    //     else
    //     {
    //         var nonPerishable = _nonPerishableDal.GetByProductId(warehouse.ProductId);
    //         warehouseModel.ShelfLife = nonPerishable.ShelfLife;
    //     }
    //
    //     return Ok(warehouseModel);
    // }
    //
    // // POST: api/warehouse/update
    // [HttpPost("update")]
    // public IActionResult Update([FromBody] WarehouseModel model)
    // {
    //     var warehouse = _warehouseDal.GetById(model.Id);
    //     if (warehouse == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     var product = _productDal.GetById(warehouse.ProductId);
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
    //         var perishable = _perishableDal.GetByProductId(warehouse.ProductId);
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
    //         var nonPerishable = _nonPerishableDal.GetByProductId(warehouse.ProductId);
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
    //     warehouse.Quantity = model.Quantity;
    //     _warehouseDal.Update(warehouse);
    //
    //     return Ok();
    // }
    
    // DELETE: api/warehouse/delete
    [HttpDelete("delete")]
    public void Delete(int id)
    {
        _warehouseDal.Delete(id);
    }
    
    // POST: api/warehouse/transfer_product
    [HttpPost("transfer_product")]
    public IActionResult MoveProduct([FromBody] WarehouseModel model)
    {
        var supermarket = _supermarketDal.GetSupermarketByTitle(model.SupermarketTitle);
        if (supermarket == null)
        {
            return NotFound("Supermarket not found.");
        }

        try
        {
            _warehouseDal.TransferFromWarehouseToInventory(model.ProductId, model.Quantity, supermarket.Id);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
        return Ok(new { message = "Transfer successful." });
    }
}