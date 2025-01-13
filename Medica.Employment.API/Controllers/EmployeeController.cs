using Medica.Employment.Domain;
using Medica.Employment.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Medica.Employment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeController> _logger;

        // Constructor injection of dependencies
        public EmployeeController(IEmployeeRepository employeeRepository, ILogger<EmployeeController> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            try
            {
                var employees = await _employeeRepository.GetAllAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all employees.");
                return StatusCode(500, "Internal server error while fetching employees.");
            }
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id);
                if (employee == null)
                {
                    _logger.LogWarning($"Employee with ID {id} not found.");
                    return NotFound($"Employee with ID {id} not found.");
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the employee with ID {id}.");
                return StatusCode(500, "Internal server error while fetching employee.");
            }
        }

        // POST: api/Employee
        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(Employee employee)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid employee model received.");
                return BadRequest(ModelState);
            }

            try
            {
                await _employeeRepository.AddAsync(employee);
                _logger.LogInformation($"Employee with ID {employee.Id} added successfully.");
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the employee.");
                return StatusCode(500, "Internal server error while adding employee.");
            }
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                _logger.LogWarning($"Employee ID mismatch: ID from URL is {id} but the employee ID is {employee.Id}.");
                return BadRequest("Employee ID mismatch.");
            }

            try
            {
                var existingEmployee = await _employeeRepository.GetByIdAsync(id);
                if (existingEmployee == null)
                {
                    _logger.LogWarning($"Employee with ID {id} not found.");
                    return NotFound($"Employee with ID {id} not found.");
                }

                await _employeeRepository.UpdateAsync(employee);
                _logger.LogInformation($"Employee with ID {id} updated successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the employee with ID {id}.");
                return StatusCode(500, "An error occurred while updating the employee.");
            }
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var existingEmployee = await _employeeRepository.GetByIdAsync(id);
                if (existingEmployee == null)
                {
                    _logger.LogWarning($"Employee with ID {id} not found.");
                    return NotFound($"Employee with ID {id} not found.");
                }

                await _employeeRepository.DeleteAsync(id);
                _logger.LogInformation($"Employee with ID {id} deleted successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the employee with ID {id}.");
                return StatusCode(500, "An error occurred while deleting the employee.");
            }
        }
    }
}
