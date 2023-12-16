namespace Payslip.Application.DTOs
{
    public class UserLoginDTO
    {
        public bool IsAdmin { get; set; }
        public int ExpiresIn { get; internal set; }
        public string AuthToken { get; internal set; }
        public string RefreshToken { get; internal set; }
        public string FullName { get; internal set; }
    }
}
