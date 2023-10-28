using Hubler.DAL.Models;

namespace Hubler.DAL.Interfaces;

public interface IAddressDAL
{
    Address GetById(int id);
    void Insert(Address address);
    void Update(Address address);
    void Delete(int id);
    List<Address> GetAll();
}