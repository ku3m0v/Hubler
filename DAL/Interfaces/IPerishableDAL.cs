using Hubler.DAL.Models;

namespace Hubler.DAL.Interfaces;

public interface IPerishableDAL
{
    Perishable GetByProductId(int productId);
    void Insert(Perishable item);
    void Update(Perishable item);
    void Delete(int productId);
    IEnumerable<Perishable> GetAll();
}