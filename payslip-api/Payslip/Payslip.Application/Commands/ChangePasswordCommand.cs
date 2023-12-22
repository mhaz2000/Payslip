using Payslip.Application.Validators;
using Payslip.Application.Validators.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payslip.Application.Commands
{
    public class ChangePasswordCommand : ICommandBase
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public void Validate() => new ChangePasswordCommandValidator().Validate(this).RaiseExceptionIfRequired();

    }
}
