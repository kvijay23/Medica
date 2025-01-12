using Medica.Employment.Domain.Entities;
using System.Net.Http;

namespace Medica.Employment.UI.Services.Contracts
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetEmployeesAsync();

        Task AddEmployee(Employee employee);

        Task DeleteEmployee(int id);
        
    }
}
