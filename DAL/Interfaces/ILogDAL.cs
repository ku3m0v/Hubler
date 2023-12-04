using Hubler.DAL.Models;

namespace Hubler.DAL.Interfaces;

public interface ILogDAL
{
    IEnumerable<Log> GetAll();
}