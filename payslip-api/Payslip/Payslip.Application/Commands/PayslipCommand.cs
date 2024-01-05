namespace Payslip.Application.Commands
{
    public class PayslipCommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CardNumber { get; set; }
        public string ContractType { get; set; }
        public string Location { get; set; }
        public string Position { get; set; }
        public IDictionary<int, string> SalaryAndBenefits { get; set; }
        public IDictionary<int, string> Durations { get; set; }
        public IDictionary<int, string> SalaryAndBenefitsAmount { get; set; }
        public IDictionary<int, string> Deductions { get; set; }
        public IDictionary<int, string> DeductionsAmount { get; set; }
        public IDictionary<int, string> Descriptions { get; set; }
        public IDictionary<int, string> DescriptionsAmount { get; set; }
        public string TotalSalaryAndBenefits { get; set; }
        public string TotalDeductions { get; set; }
        public string NetPayable { get; set; }
    }
}
