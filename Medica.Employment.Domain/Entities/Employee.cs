
namespace Medica.Employment.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string JobTitle { get; set; }
        public string Team { get; set; }
        public string LineManager { get; set; }
        public DateTime StartDate { get; set; }
        public string ProfilePicture { get; set; }
    }
}

