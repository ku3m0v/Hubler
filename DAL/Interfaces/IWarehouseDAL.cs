using Hubler.DAL.Models;

namespace Hubler.DAL.Interfaces;

public interface IWarehouseDAL
{
    Warehouse GetById(int id);
    void Insert(Warehouse warehouse);
    void Update(Warehouse warehouse);
    void Delete(int id);
    IEnumerable<Warehouse> GetAll();
    void TransferFromWarehouseToInventory(int productId, int quantity, int supermarketId);
}