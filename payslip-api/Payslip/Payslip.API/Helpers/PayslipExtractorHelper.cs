using OfficeOpenXml;
using Payslip.API.Base;
using Payslip.Application.Commands;

namespace Payslip.API.Helpers
{
    public class ExcelHelpler : IExcelHelpler
    {
        public IEnumerable<UserModel> ExtractUsers(Stream stream)
        {

            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                worksheet.TrimLastEmptyRows();
                var rowCount = worksheet.Dimension.Rows;

                for (int i = 2; i < rowCount; i++)
                {
                    yield return new UserModel()
                    {
                        FirstName = worksheet.Cells[i, 2].Value.ToString(),
                        LastName = worksheet.Cells[i, 3].Value.ToString(),
                        NationalCode = worksheet.Cells[i, 4].Value.ToString(),
                        CardNumber = worksheet.Cells[i, 5].Value.ToString(),
                    };
                }
            }
        }

        public IEnumerable<PayslipCommand> ExtractPayslips(Stream stream)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                worksheet.TrimLastEmptyRows();
                var rowCount = worksheet.Dimension.Rows;

                int? counter = FindNextRow(worksheet, 1, rowCount);
                while (counter is not null)
                {
                    var payslipCommand = GeneratePayslip(worksheet, counter.Value);
                    yield return payslipCommand;

                    counter = FindNextRow(worksheet, counter.Value + 1, rowCount);
                }
            }
        }

        private int? FindNextRow(ExcelWorksheet worksheet, int index, int rowCount)
        {
            while (index <= rowCount)
            {
                if (worksheet.Cells[index, 1].Value?.ToString()?.Contains("رديـف :") ?? false)
                    return index;

                index++;
            }

            return null;
        }

        private PayslipCommand GeneratePayslip(ExcelWorksheet worksheet, int index)
        {
            var lastName = worksheet.Cells[index + 2, 3].Value?.ToString()!.Replace("نـام خانوادگـي : ", "");
            var firstName = worksheet.Cells[index + 2, 6].Value?.ToString()!.Replace("نـام : ", "");
            var cardNumber = worksheet.Cells[index + 3, 1].Value?.ToString()!.Replace("شـماره كارت : ", "");
            var contractType = worksheet.Cells[index + 4, 1].Value?.ToString()!.Replace("نـوع استخـدام : ", "");
            var location = worksheet.Cells[index + 4, 6].Value?.ToString()!;
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



            var totalSalaryAndBenefits = worksheet.Cells[index + 20, 4].Value?.ToString()!;
            var totaldeductions = worksheet.Cells[index + 20, 6].Value?.ToString()!;
            var netPayable = worksheet.Cells[index + 20, 8].Value?.ToString()!;

            return new PayslipCommand()
            {
                CardNumber = cardNumber,
                ContractType = contractType,
                Deductions = deductions,
                DeductionsAmount = deductionsAmount,
                Durations = durations,
                FirstName = firstName,
                LastName = lastName,
                Location = location,
                NetPayable = netPayable,
                Position = position,
                SalaryAndBenefits = salaryAndBenefits,
                SalaryAndBenefitsAmount = salaryAndBenefitsAmounts,
                TotalDeductions = totaldeductions,
                TotalSalaryAndBenefits = totalSalaryAndBenefits
            };
        }
    }
}
