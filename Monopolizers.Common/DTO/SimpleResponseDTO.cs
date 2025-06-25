using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Common.DTO
{
    public class SimpleResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public SimpleResponseDTO(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
