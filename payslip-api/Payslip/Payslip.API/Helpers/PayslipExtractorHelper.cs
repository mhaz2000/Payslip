using OfficeOpenXml;
using Payslip.Application.Commands;
using System.Diagnostics.Metrics;

namespace Payslip.API.Helpers
{
    public class PayslipExtractorHelper : IPayslipExtractorHelpler
    {
        public IEnumerable<PayslipCommand> Extract(Stream stream)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                worksheet.TrimLastEmptyRows();
                var rowCount = worksheet.Dimension.Rows;

                int counter = 2;
                while (counter <= rowCount)
                {
                    var payslipCommand = GeneratePayslip(worksheet, counter);
                    counter += 23;

                    yield return payslipCommand;
                }
            }
        }

        private PayslipCommand GeneratePayslip(ExcelWorksheet worksheet, int index)
        {
            var number = worksheet.Cells[index, 1].Value?.ToString()!.Replace("رديـف :", "");
            var date = worksheet.Cells[index, 8].Value?.ToString()!.Replace("تاريـخ : ", "");
            var detailAccountCode = worksheet.Cells[index + 2, 1].Value?.ToString()!.Replace("كـد تفصيلـي : ", "");
            var lastName = worksheet.Cells[index + 2, 3].Value?.ToString()!.Replace("نـام خانوادگـي : ", "");
            var firstName = worksheet.Cells[index + 2, 6].Value?.ToString()!.Replace("نـام : ", "");
            var cardNumber = worksheet.Cells[index + 3, 1].Value?.ToString()!.Replace("شـماره كارت : ", "");
            var personellCode = worksheet.Cells[index + 3, 3].Value?.ToString()!.Replace("شـماره پرسنلي :", "");
            var accountNumber = worksheet.Cells[index + 3, 5].Value?.ToString()!.Replace("شـماره حساب : ", "");
            var bank = worksheet.Cells[index + 3, 6].Value?.ToString()!.Replace("بـانك : ", "");
            var dailySalary = worksheet.Cells[index + 3, 7].Value?.ToString()!.Replace("دستمزد روزانه : ", "");
            var contractType = worksheet.Cells[index + 4, 1].Value?.ToString()!.Replace("نـوع استخـدام : ", "");
            var location = worksheet.Cells[index + 4, 6].Value?.ToString()!;
            var costCenter = worksheet.Cells[index + 5, 1].Value?.ToString()!.Replace("مـركـز هزينـه : ", "");
            var position = worksheet.Cells[index + 5, 6].Value?.ToString()!;

            int counter = 0;
            Dictionary<int, string> salaryAndBenefits = new Dictionary<int, string>();
            do
            {
                salaryAndBenefits.Add(counter + 1, worksheet.Cells[index + 8 + counter, 1].Value?.ToString()!);
                counter++;
            }
            while (worksheet.Cells[index + 8 + counter, 1].Value is not null &&
                !string.IsNullOrEmpty(worksheet.Cells[index + 8 + counter, 1].Value.ToString()) &&
                (!worksheet.Cells[index + 8 + counter, 1].Value?.ToString()!.Contains("جمـع كـل حقـوق و مـزايا") ?? true));


            counter = 0;
            Dictionary<int, string> durations = new Dictionary<int, string>();
            do
            {
                durations.Add(counter + 1, worksheet.Cells[index + 8 + counter, 3].Value?.ToString()!);
                counter++;
            }
            while (worksheet.Cells[index + 8 + counter, 3].Value is not null &&
                (!string.IsNullOrEmpty(worksheet.Cells[index + 8 + counter, 3].Value.ToString())) &&
                (!worksheet.Cells[index + 8 + counter, 1].Value?.ToString()!.Contains("جمـع كـل حقـوق و مـزايا") ?? true));


            counter = 0;
            Dictionary<int, string> salaryAndBenefitsAmounts = new Dictionary<int, string>();
            do
            {
                salaryAndBenefitsAmounts.Add(counter + 1, worksheet.Cells[index + 8 + counter, 4].Value?.ToString()!);
                counter++;
            }
            while (worksheet.Cells[index + 8 + counter, 4].Value is not null &&
                (!string.IsNullOrEmpty(worksheet.Cells[index + 8 + counter, 4].Value.ToString())) &&
                (!worksheet.Cells[index + 8 + counter, 1].Value?.ToString()!.Contains("جمـع كـل حقـوق و مـزايا") ?? true));

            counter = 0;
            Dictionary<int, string> deductions = new Dictionary<int, string>();
            do
            {
                deductions.Add(counter + 1, worksheet.Cells[index + 8 + counter, 5].Value?.ToString()!);
                counter++;
            }
            while (worksheet.Cells[index + 8 + counter, 5].Value is not null &&
                (!string.IsNullOrEmpty(worksheet.Cells[index + 8 + counter, 5].Value.ToString())) &&
                (!worksheet.Cells[index + 8 + counter, 1].Value?.ToString()!.Contains("جمـع كـل حقـوق و مـزايا") ?? true));

            counter = 0;
            Dictionary<int, string> deductionsAmount = new Dictionary<int, string>();
            do
            {
                deductionsAmount.Add(counter + 1, worksheet.Cells[index + 8 + counter, 6].Value?.ToString()!);
                counter++;
            }
            while (worksheet.Cells[index + 8 + counter, 6].Value is not null &&
                (!string.IsNullOrEmpty(worksheet.Cells[index + 8 + counter, 6].Value.ToString())) &&
                (!worksheet.Cells[index + 8 + counter, 1].Value?.ToString()!.Contains("جمـع كـل حقـوق و مـزايا") ?? true));

            counter = 0;
            Dictionary<int, string> descriptions = new Dictionary<int, string>();
            do
            {
                descriptions.Add(counter + 1, worksheet.Cells[index + 8 + counter, 7].Value?.ToString()!);
                counter++;
            }
            while (worksheet.Cells[index + 8 + counter, 7].Value is not null &&
                (!string.IsNullOrEmpty(worksheet.Cells[index + 8 + counter, 7].Value.ToString())) &&
                (!worksheet.Cells[index + 8 + counter, 1].Value?.ToString()!.Contains("جمـع كـل حقـوق و مـزايا") ?? true));

            counter = 0;
            Dictionary<int, string> descriptionsAmount = new Dictionary<int, string>();
            do
            {
                descriptionsAmount.Add(counter + 1, worksheet.Cells[index + 8 + counter, 8].Value?.ToString()!);
                counter++;
            }
            while (worksheet.Cells[index + 8 + counter, 8].Value is not null &&
                (!string.IsNullOrEmpty(worksheet.Cells[index + 8 + counter, 8].Value.ToString())) &&
                 (!worksheet.Cells[index + 8 + counter, 1].Value?.ToString()!.Contains("جمـع كـل حقـوق و مـزايا") ?? true));


            var totalSalaryAndBenefits = worksheet.Cells[index + 20, 4].Value?.ToString()!;
            var totaldeductions = worksheet.Cells[index + 20, 6].Value?.ToString()!;
            var netPayable = worksheet.Cells[index + 20, 8].Value?.ToString()!;

            return new PayslipCommand()
            {
                AccountNumber = accountNumber,
                Bank = bank,
                CardNumber = cardNumber,
                ContractType = contractType,
                CostCenter = costCenter,
                DailySalary = dailySalary,
                Date =date,
                Deductions = deductions,
                DeductionsAmount = deductionsAmount,
                Descriptions = descriptions,
                DescriptionsAmount= descriptionsAmount,
                DetailAccountCode = detailAccountCode,
                Durations = durations,
                FirstName = firstName,
                LastName = lastName,
                Location= location,
                NetPayable = netPayable,
                PersonellCode = personellCode,
                Position= position,
                SalaryAndBenefits= salaryAndBenefits,
                SalaryAndBenefitsAmount = salaryAndBenefitsAmounts,
                TotalDeductions = totaldeductions,
                TotalSalaryAndBenefits = totalSalaryAndBenefits
            };
        }
    }
}
