using Payslip.Core.Enums;
using System;

namespace Payslip.Core.Entities
{
    public class UserPayslip : BaseEntity
    {
        public UserPayslip() : base()
        {

        }

        public User? User { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CardNumber { get; set; }
        public string? Bank { get; set; }
        public string? ContractType { get; set; }
        public string? Location { get; set; }
        public string? Position { get; set; }
        public string? TotalSalaryAndBenefits { get; set; }
        public string? TotalDeductions { get; set; }
        public string? NetPayable { get; set; }
        public int Year { get; set; }
        public Month Month { get; set; }

        public IDictionary<int, string>? SalaryAndBenefits { get; set; }
        public IDictionary<int, string>? Durations { get; set; }
        public IDictionary<int, string>? SalaryAndBenefitsAmount { get; set; }
        public IDictionary<int, string>? Deductions { get; set; }
        public IDictionary<int, string>? DeductionsAmount { get; set; }

    }
}
