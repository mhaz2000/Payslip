using Microsoft.AspNetCore.Mvc;

namespace Payslip_Api.Helpers
{
    public class ActionResultHelper
    {
        public static T GetObjectResultContent<T>(IActionResult result)
        {
            return (T)((ObjectResult)result).Value!;
        }
    }
}
