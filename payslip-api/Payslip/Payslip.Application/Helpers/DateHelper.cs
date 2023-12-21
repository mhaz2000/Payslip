using Payslip.Application.Base;
using Payslip.Core.Enums;

namespace Payslip.Application.Helpers
{
    public class DateHelper
    {
        public static (int,Month) ValidateDate(int year, int month)
        {
            if (year < 1400 || year > 1500)
                throw new ManagedException("سال مالی صحیح نمی باشد.");

            if (month < 1 || month > 12)
                throw new ManagedException("ماه صحیح نمی باشد.");

            var parsedMonth = (Month)month;
            return (year, parsedMonth);
        }
    }
}
