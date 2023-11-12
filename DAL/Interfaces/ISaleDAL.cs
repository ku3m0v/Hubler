using Hubler.DAL.Models;

namespace Hubler.DAL.Interfaces;

public interface ISaleDAL
{
    Sale GetById(int id);
    void Insert(Sale sale);
    void Update(Sale sale);
    void Delete(int id);
    IEnumerable<Sale> GetAll();
}