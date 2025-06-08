using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Common.DTO
{
    public class UserSavedCardDTO
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public string Background { get; set; } = null!;
        public string ElementsJson { get; set; } = null!;
        public DateTime SavedAt { get; set; }
    }
}
