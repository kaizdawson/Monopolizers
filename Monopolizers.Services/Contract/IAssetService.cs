using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monopolizers.Common.DTO;

namespace Monopolizers.Service.Contract
{
    public interface IAssetService
    {
        Task<ResponseDTO> GetAllAssetsAsync();
    }
}
