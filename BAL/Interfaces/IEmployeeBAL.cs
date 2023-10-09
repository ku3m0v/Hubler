namespace Hubler.BAL.Interfaces;

public interface IEmployeeBAL
{
    int? Authenticate(string email, string password);
}