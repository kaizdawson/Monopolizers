using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Common.Helpers
{
    public class EmailSettings : IEmailSettings
    {
        public string Email { get; set; }          // khớp appsettings
        public string DisplayName { get; set; }    // khớp appsettings
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
