namespace Payslip.Application.DTOs
{
    public class PayslipDTO
    {
        public string Month { get; set; }
        public int Year { get; set; }
        public DateTime UploadedDate { get; set; }
        public Guid FileId { get; set; }
    }
}
