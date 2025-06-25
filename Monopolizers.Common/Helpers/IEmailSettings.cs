using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Common.Helpers
{
    public interface IEmailSettings
    {
        string Email { get; set; }
        string DisplayName { get; set; }
        string Password { get; set; }
        string Host { get; set; }
        int Port { get; set; }
    }
}
