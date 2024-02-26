using Microsoft.Extensions.FileSystemGlobbing.Internal;
using OfficeOpenXml;
using Payslip.API.Base;
using Payslip.Application.Commands;
using Payslip.Infrastructure.Migrations;
using System.Text.RegularExpressions;

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

                for (int i = 2; i <= rowCount; i++)
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
                var rowCount = worksheet.Dimension.Rows+1;

                int? counter = FindNextRow(worksheet, 1, rowCount);
                while (counter is not null)
                {
                    var payslipCommand = GeneratePayslip(worksheet, counter.Value, rowCount);
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

        private PayslipCommand GeneratePayslip(ExcelWorksheet worksheet, int index, int rowCount)
        {
            string pattern = @"\[\d+\]";

            var lastName = worksheet.Cells[index + 2, 3].Value?.ToString()!.Replace("نـام خانوادگـي : ", "");
            var firstName = worksheet.Cells[index + 2, 6].Value?.ToString()!.Replace("نـام : ", "");
            var cardNumber = worksheet.Cells[index + 3, 1].Value?.ToString()!.Replace("شـماره كارت : ", "");

            var contractType = Regex.Replace(worksheet.Cells[index + 4, 1].Value?.ToString()!.Replace("نـوع استخـدام : ", "")!, pattern, "");
            var location = Regex.Replace(worksheet.Cells[index + 4, 6].Value?.ToString()!, pattern, "");
            var position = Regex.Replace(worksheet.Cells[index + 5, 6].Value?.ToString()!, pattern, "");


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
                if (MustConsiderDuration(worksheet.Cells[index + 8 + counter, 1].Value?.ToString()))
                    durations.Add(counter + 1, worksheet.Cells[index + 8 + counter, 3].Value?.ToString()!);
                else
                    durations.Add(counter + 1, "");

                counter++;
            }
            while (worksheet.Cells[index + 8 + counter, 3].Value is not null &&
                (!string.IsNullOrEmpty(worksheet.Cells[index + 8 + counter, 3].Value.ToString())) &&
                (!worksheet.Cells[index + 8 + counter, 1].Value?.ToString()!.Contains("جمـع كـل حقـوق و مـزايا") ?? true));


            counter = 0;
            Dictionary<int, string> salaryAndBenefitsAmounts = new Dictionary<int, string>();
            do
            {
                salaryAndBenefitsAmounts.Add(counter + 1, ulong.TryParse(worksheet.Cells[index + 8 + counter, 4].Value?.ToString()!, out ulong salaryBenefit) ?
                    salaryBenefit.ToString("n0") : "0");
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
                deductionsAmount.Add(counter + 1, ulong.TryParse(worksheet.Cells[index + 8 + counter, 6].Value?.ToString()!, out ulong deductionAmount) ?
                    deductionAmount.ToString("n0") : "0");
                counter++;
            }
            while (worksheet.Cells[index + 8 + counter, 6].Value is not null &&
                (!string.IsNullOrEmpty(worksheet.Cells[index + 8 + counter, 6].Value.ToString())) &&
                (!worksheet.Cells[index + 8 + counter, 1].Value?.ToString()!.Contains("جمـع كـل حقـوق و مـزايا") ?? true));

            counter = 0;
            Dictionary<int, string> descriptions = new Dictionary<int, string>();
            Dictionary<int, string> descriptionsAmount = new Dictionary<int, string>();
            if (worksheet.Cells[index + 8 + counter, 7].Value is not null &&
                (!string.IsNullOrEmpty(worksheet.Cells[index + 8 + counter, 7].Value.ToString())) &&
                (worksheet.Cells[index + 8 + counter, 7].Value.ToString()!.Contains("مشمول ماليات حقوق")) &&
                (!worksheet.Cells[index + 8 + counter, 1].Value?.ToString()!.Contains("جمـع كـل حقـوق و مـزايا") ?? true))
            {
                descriptions.Add(counter + 1, worksheet.Cells[index + 8 + counter, 7].Value?.ToString()!);
                descriptionsAmount.Add(
                    counter + 1, ulong.TryParse(worksheet.Cells[index + 8 + counter, 8].Value?.ToString()!, out ulong descriptionAmount) ? descriptionAmount.ToString("n0") : "0");
                counter++;
            }

            if (worksheet.Cells[index + 8 + counter, 7].Value is not null &&
                (!string.IsNullOrEmpty(worksheet.Cells[index + 8 + counter, 7].Value.ToString())) &&
                (worksheet.Cells[index + 8 + counter, 7].Value.ToString()!.Contains("مشمول بيمه حقوق")) &&
                (!worksheet.Cells[index + 8 + counter, 1].Value?.ToString()!.Contains("جمـع كـل حقـوق و مـزايا") ?? true))
            {
                descriptions.Add(counter + 1, worksheet.Cells[index + 8 + counter, 7].Value?.ToString()!);
                descriptionsAmount.Add(
                    counter + 1, ulong.TryParse(worksheet.Cells[index + 8 + counter, 8].Value?.ToString()!, out ulong descriptionAmount) ? descriptionAmount.ToString("n0") : "0");
            }

            int sumRow = index + 8;
            while (sumRow < rowCount && (!worksheet.Cells[sumRow, 1].Value?.ToString()!.Contains("جمـع كـل حقـوق و مـزايا") ?? true))
                sumRow++;

            var totalSalaryAndBenefits = ulong.TryParse(worksheet.Cells[sumRow, 4].Value?.ToString()!, out ulong totalSalaryAndBenefit)
                ? totalSalaryAndBenefit.ToString("n0") : "0";
            var totalDeductions = ulong.TryParse(worksheet.Cells[sumRow, 6].Value?.ToString()!, out ulong totalDeduction) ? totalDeduction.ToString("n0") : "0";
            var netPayable = ulong.TryParse(worksheet.Cells[sumRow, 8].Value?.ToString()!, out ulong parsedNetPayable) ? parsedNetPayable.ToString("n0") : "0";

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
                TotalDeductions = totalDeductions,
                TotalSalaryAndBenefits = totalSalaryAndBenefits,
                Descriptions = descriptions,
                DescriptionsAmount = descriptionsAmount
            };
        }

        private bool MustConsiderDuration(string? value)
        {
            if (value is null)
                return false;

            if (value.Contains("اضافه کار") || value.Contains("ناهار") || value.Contains("اضافه كار") || value.Contains("جمعه کار") || value.Contains("تعطيل کار") || value.Contains("دستمزد"))
                return true;

            return false;
        }
    }
}
