using Microsoft.AspNetCore.Identity;

namespace Payslip.Core.Entities
{
    public class User : IdentityUser<Guid>
    {
        public User(string lastName, string firstName, string nationalCode, string personnelCode)
        {
            LastName = lastName;
            FirstName = firstName;
            NationalCode = nationalCode;
            PersonnelCode = personnelCode;
            Id = Guid.NewGuid();
        }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string NationalCode { get; set; }
        public string PersonnelCode { get; set; }
    }
}
