using Medica.Employment.Domain.Entities;

namespace Medica.Employment.Domain
{
    public interface IEmployeeRepository1
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(int id);
    }
}
