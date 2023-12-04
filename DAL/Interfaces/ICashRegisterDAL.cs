using Hubler.DAL.Models;

namespace Hubler.DAL.Interfaces;

public interface ICashRegisterDAL
{
    CashRegister GetById(int id);
    void Insert(CashRegister cashRegister);
    void Update(CashRegister cashRegister);
    void Delete(int id);
    IEnumerable<CashRegister> GetAll();
    CashRegister GetBySupermarketIdAndRegisterNumber(int supermarketId, int registerNumber);
}