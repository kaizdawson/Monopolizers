using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monopolizers.Common.Enums;

namespace Monopolizers.Common.DTO
{
    public class CardDTO
    {
        public int CardId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? AvtImgUrl { get; set; }
        public string? ARVideoUrl { get; set; }
        public int Category { get; set; }
        public int TypeId { get; set; }
        public string? DefaultData { get; set; }
        public AccessLevelEnum AccessLevel { get; set; }
    }
}
