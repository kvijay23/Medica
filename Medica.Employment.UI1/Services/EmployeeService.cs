using Medica.Employment.Domain.Entities;
using Medica.Employment.UI.Services.Contracts;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Medica.Employment.UI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(HttpClient httpClient, ILogger<EmployeeService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all employees.
        /// </summary>
        public async Task<List<Employee>> GetEmployeesAsync()
        {
            try
            {
                var employees = await _httpClient.GetFromJsonAsync<List<Employee>>("api/employee");

                // If the response is null, return an empty list
                return employees ?? new List<Employee>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching employees.");
                throw new ApplicationException("Failed to retrieve employees. Please try again later.", ex);
            }
        }

        /// <summary>
        /// Adds a new employee.
        /// </summary>
        public async Task AddEmployee(Employee employee)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/employee", employee);
                response.EnsureSuccessStatusCode();  

                _logger.LogInformation($"Employee {employee.FirstName} {employee.LastName} added successfully.");
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Request error occurred while adding an employee.");
                throw new ApplicationException("Failed to add employee. Please try again later.", e);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "General error occurred while adding an employee.");
                throw new ApplicationException("An error occurred while adding the employee.", e);
            }
        }

        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        public async Task UpdateEmployee(Employee employee)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/employee/{employee.Id}", employee);
                response.EnsureSuccessStatusCode();  

                _logger.LogInformation($"Employee {employee.FirstName} {employee.LastName} updated successfully.");
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Request error occurred while updating an employee.");
                throw new ApplicationException("Failed to update employee. Please try again later.", e);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "General error occurred while updating an employee.");
                throw new ApplicationException("An error occurred while updating the employee.", e);
            }
        }

        /// <summary>
        /// Deletes an employee by ID.
        /// </summary>
        public async Task DeleteEmployee(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/employee/{id}");
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Request error occurred while deleting an employee.");
                throw new ApplicationException("Failed to delete employee. Please try again later.", e);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "General error occurred while deleting an employee.");
                throw new ApplicationException("An error occurred while deleting the employee.", e);
            }
        }
    }
}
