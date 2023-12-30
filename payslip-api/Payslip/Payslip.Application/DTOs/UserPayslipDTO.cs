using Payslip.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payslip.Application.DTOs
{
    public class UserPayslipDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CardNumber { get; set; }
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

        public Guid FileId { get; set; }
    }
}
