using Microsoft.AspNetCore.Identity;

namespace Payslip.Core.Entities
{
    public class User : IdentityUser<Guid>
    {
        public User()
        {

        }
        public User(string username, string lastName, string firstName, string nationalCode, string cardNumber)
        {
            LastName = lastName;
            FirstName = firstName;
            NationalCode = nationalCode;
            CardNumber = cardNumber;
            UserName= username;
            IsActive= true;
            Id = Guid.NewGuid();
        }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string NationalCode { get; set; }
        public string CardNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
