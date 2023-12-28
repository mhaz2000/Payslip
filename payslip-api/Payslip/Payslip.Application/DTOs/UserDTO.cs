namespace Payslip.Application.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string CardNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
