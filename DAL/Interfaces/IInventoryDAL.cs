using Hubler.DAL.Models;

namespace Hubler.DAL.Interfaces;

public interface IInventoryDAL
{
    Inventory GetById(int id);
    void Insert(Inventory inventory);
    void Update(Inventory inventory);
    void Delete(int id);
    IEnumerable<Inventory> GetAll();
}