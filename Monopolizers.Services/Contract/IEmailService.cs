using Monopolizers.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Service.Contract
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDTO emailDto);
    }
}
