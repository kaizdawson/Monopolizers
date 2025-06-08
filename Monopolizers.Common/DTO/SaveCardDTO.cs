using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Common.DTO
{
    public class SaveCardDTO
    {
        public int CardId { get; set; }
        public string Background { get; set; } = null!;
        public string ElementsJson { get; set; } = null!; // JSON chứa text/sticker
    }
}
