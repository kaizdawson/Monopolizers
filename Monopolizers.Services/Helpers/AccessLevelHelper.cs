using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Monopolizers.Common.Enums;

namespace Monopolizers.Service.Helpers
{
    public static class AccessLevelHelper
    {
        public static AccessLevelEnum GetAccessLevelFromClaims(ClaimsPrincipal user)
        {
            var claimValue = user.FindFirst("accessLevel")?.Value ?? "Basic";
            return Enum.TryParse(claimValue, out AccessLevelEnum level) ? level : AccessLevelEnum.Basic;
        }
    }
}
