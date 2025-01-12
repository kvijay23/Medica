using Medica.Employment.Domain;
using Medica.Employment.Domain.Entities;

namespace Medica.Employment.Infrastructure.Repositories
{
    public class EmployeeRepositoryCsv : IEmployeeRepository
    {
        private readonly string _filePath;
        private const string CsvHeader = "Id,FirstName,LastName,Email,Telephone,DateOfBirth,Address1,Address2,Town,County,Postcode,JobTitle,Team,LineManager,StartDate,ProfilePicture";
        private const int MinimumColumns = 15;

        public EmployeeRepositoryCsv(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var employees = new List<Employee>();

            // Read all lines from the file
            var lines = await File.ReadAllLinesAsync(_filePath);

            if (lines.Length <= 1)
                return employees; // Return empty if no data

            foreach (var line in lines.Skip(1)) // Skip header
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var values = line.Split(',');

                if (values.Length < MinimumColumns)
                    continue; // Skip invalid rows

                employees.Add(ParseEmployee(values));
            }

            return employees;
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            var employees = await GetAllAsync();
            return employees.FirstOrDefault(e => e.Id == id);
        }

        public async Task AddAsync(Employee employee)
        {
            var employees = (await GetAllAsync()).ToList();
            employee.Id = employees.Any() ? employees.Max(e => e.Id) + 1 : 1; // Auto-increment ID

            employees.Add(employee);
            await WriteAllEmployeesAsync(employees);
        }

        public async Task UpdateAsync(Employee employee)
        {
            var employees = (await GetAllAsync()).ToList();
            var index = employees.FindIndex(e => e.Id == employee.Id);

            if (index != -1)
            {
                employees[index] = employee;
                await WriteAllEmployeesAsync(employees);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var employees = (await GetAllAsync()).ToList();
            var employee = employees.FirstOrDefault(e => e.Id == id);

            if (employee != null)
            {
                employees.Remove(employee);
                await WriteAllEmployeesAsync(employees);
            }
        }

        // Helper function to write all employees to CSV
        private async Task WriteAllEmployeesAsync(IEnumerable<Employee> employees)
        {
            var lines = new List<string> { CsvHeader };
            lines.AddRange(employees.Select(e => ConvertToCsvLine(e)));
            await File.WriteAllLinesAsync(_filePath, lines);
        }

        // Helper function to parse an employee from CSV values
        private Employee ParseEmployee(string[] values)
        {
            return new Employee
            {
                Id = int.TryParse(values[0], out var id) ? id : 0, // Parse ID or set to 0
                FirstName = values[1],
                LastName = values[2],
                Email = values[3],
                Telephone = values[4],
                DateOfBirth = DateTime.TryParse(values[5], out var dob) ? dob : DateTime.Now, // Default to current date if invalid
                Address1 = values[6],
                Address2 = values[7],
                Town = values[8],
                County = values[9],
                Postcode = values[10],
                JobTitle = values[11],
                Team = values[12],
                LineManager = string.IsNullOrWhiteSpace(values[13]) ? null : values[13], // Handle missing LineManager
                StartDate = DateTime.TryParse(values[14], out var startDate) ? startDate : DateTime.Now, // Default to current date if invalid
                ProfilePicture = values[15]
            };
        }

        // Helper function to convert employee to CSV line
        private string ConvertToCsvLine(Employee employee)
        {
            return $"{employee.Id},{employee.FirstName},{employee.LastName},{employee.Email},{employee.Telephone},{employee.DateOfBirth:yyyy-MM-dd},{employee.Address1},{employee.Address2},{employee.Town},{employee.County},{employee.Postcode},{employee.JobTitle},{employee.Team},{employee.LineManager},{employee.StartDate:yyyy-MM-dd},{employee.ProfilePicture}";
        }
    }
}
