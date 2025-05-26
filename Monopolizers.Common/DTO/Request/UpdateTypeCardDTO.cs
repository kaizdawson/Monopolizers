using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Common.DTO.Request
{
    public class UpdateTypeCardDTO
    {
        public int TypeId { get; set; }
        public string Name { get; set; } = null!;
    }
}
