using Hubler.DAL.Models;

namespace Hubler.DAL.Interfaces;

public interface ISaleDAL
{
    Sale GetById(int id);
    int Insert(Sale sale);
    void Update(Sale sale);
    void Delete(int id);
    IEnumerable<Sale> GetAll();
}