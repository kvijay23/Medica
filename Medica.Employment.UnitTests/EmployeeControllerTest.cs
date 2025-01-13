using Microsoft.Extensions.DependencyInjection;
using Moq;
using Medica.Employment.Domain.Entities;
using Medica.Employment.Domain;
using Medica.Employment.Infrastructure.Repositories;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Medica.Employment.API.Controllers;
using Microsoft.Extensions.Logging;

namespace Medica.Employment.Tests
{
    public class EmployeeControllerTests
    {
        private readonly EmployeeController _controller;
        private readonly Mock<IEmployeeRepository> _mockRepo;
        private readonly Mock<ILogger<EmployeeController>> _mockLogger;

        public EmployeeControllerTests()
        {
            // Initialize the mock repository and logger
            _mockRepo = new Mock<IEmployeeRepository>();
            _mockLogger = new Mock<ILogger<EmployeeController>>();

            // Create the controller with the mocked repository and logger
            _controller = new EmployeeController(_mockRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetEmployees_ReturnsAllEmployees()
        {
            // Arrange
            var employees = GetSampleEmployees(); // Using the sample employee data
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(employees);

            // Act
            var result = await _controller.GetEmployees();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Employee>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedEmployees = Assert.IsType<List<Employee>>(okResult.Value);
            Assert.Equal(3, returnedEmployees.Count);  // Assuming 3 records in the mock
        }

        [Fact]
        public async Task GetEmployee_ReturnsSingleEmployee()
        {
            // Arrange
            var employees = GetSampleEmployees();
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => employees.FirstOrDefault(e => e.Id == id));

            // Act
            var result = await _controller.GetEmployee(1); // Request employee with ID 1

            // Assert
            var actionResult = Assert.IsType<ActionResult<Employee>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var employee = Assert.IsType<Employee>(okResult.Value);
            Assert.Equal(1, employee.Id);
            Assert.Equal("John", employee.FirstName);
        }

        [Fact]
        public async Task AddEmployee_CreatesNewEmployee()
        {
            // Arrange
            var newEmployee = new Employee
            {
                FirstName = "Alice",
                LastName = "Doe",
                Email = "alice.doe@medica.co.uk",
                Telephone = "07123456789",
                DateOfBirth = new DateTime(1995, 10, 5),
                Address1 = "12 Oak Street",
                Address2 = "",
                Town = "Greenwich",
                County = "Kent",
                Postcode = "GR5 6NH",
                JobTitle = "Manager",
                Team = "Finance",
                LineManager = "John Smith",
                StartDate = new DateTime(2023, 5, 1),
                ProfilePicture = "profile4.png"
            };

            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Employee>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddEmployee(newEmployee);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Employee>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var createdEmployee = Assert.IsType<Employee>(createdResult.Value);
            Assert.Equal("Alice", createdEmployee.FirstName);
        }

     
        [Fact]
        public async Task DeleteEmployee_DeletesEmployeeSuccessfully()
        {
            // Arrange
            var employeeToDelete = new Employee
            {
                Id = 4,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@medica.co.uk",
                Telephone = "07123456789",
                DateOfBirth = new DateTime(1990, 10, 5),
                Address1 = "12 Oak Street",
                Address2 = "Greenwich",
                Town = "Greenwich",
                County = "Kent",
                Postcode = "GR5 6NH",
                JobTitle = "Manager",
                Team = "Finance",
                LineManager = "John Smith",
                StartDate = new DateTime(2023, 5, 1),
                ProfilePicture = "profile4.png"
            };

            _mockRepo.Setup(repo => repo.GetByIdAsync(4)).ReturnsAsync(employeeToDelete);
            _mockRepo.Setup(repo => repo.DeleteAsync(4)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteEmployee(4);
            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result);

            // Verify repository methods were called
            _mockRepo.Verify(repo => repo.GetByIdAsync(4), Times.Once());
            _mockRepo.Verify(repo => repo.DeleteAsync(4), Times.Once());
        }

        // Helper function to return sample data
        private static IEnumerable<Employee> GetSampleEmployees()
        {
            return new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "johnsmith@medica.co.uk",
                    Telephone = "07894000111",
                    DateOfBirth = new DateTime(1990, 12, 20),
                    Address1 = "1 Main Street",
                    Town = "Townsville",
                    County = "Big County",
                    Postcode = "TN12 3AB",
                    JobTitle = "Purchasing Assistant",
                    Team = "Finance",
                    LineManager = "Sienna Lin",
                    StartDate = new DateTime(2023, 10, 31),
                    ProfilePicture = "profile2.png"
                },
                new Employee
                {
                    Id = 2,
                    FirstName = "Simon",
                    LastName = "Trelawny",
                    Email = "simontrelawny@medica.co.uk",
                    Telephone = "07456236458",
                    DateOfBirth = new DateTime(1984, 8, 5),
                    Address1 = "First floor flat",
                    Town = "Smalltown",
                    County = "Small County",
                    Postcode = "ST12 5LH",
                    JobTitle = "Software Engineer",
                    Team = "Software Development",
                    LineManager = "Ann Hitch",
                    StartDate = new DateTime(2012, 7, 12),
                    ProfilePicture = "profile3.png"
                },
                new Employee
                {
                    Id = 3,
                    FirstName = "Barbara",
                    LastName = "Johnson",
                    Email = "barbarajohnson@medica.co.uk",
                    Telephone = "014245687489",
                    DateOfBirth = new DateTime(1976, 9, 16),
                    Address1 = "The House",
                    Town = "Bleakstone",
                    County = "Middle County",
                    Postcode = "BS85 7GH",
                    JobTitle = "Chief Executive Officer",
                    Team = "Senior Management",
                    LineManager = "",
                    StartDate = new DateTime(2001, 9, 16),
                    ProfilePicture = "profile1.png"
                }
            };
        }
    }
}
