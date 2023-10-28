using Hubler.DAL.Models;

namespace Hubler.DAL.Interfaces;

public interface ISaleDetailDAL
{
    SaleDetail GetById(int id);
    void Insert(SaleDetail saleDetail);
    void Update(SaleDetail saleDetail);
    void Delete(int id);
    IEnumerable<SaleDetail> GetAll();
}