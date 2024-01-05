using Payslip.Application.Validators;
using Payslip.Application.Validators.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payslip.Application.Commands
{
    public class ChangePasswordByAdminCommand : ICommandBase
    {
        public Guid UserId { get; set; }
        public string NewPassword { get; set; }
        public void Validate() => new ChangePasswordByAdminCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
