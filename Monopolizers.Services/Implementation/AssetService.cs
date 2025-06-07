using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Monopolizers.Common.BusinessCode;
using Monopolizers.Common.DTO;
using Monopolizers.Repository.Contract;
using Monopolizers.Repository.DB;
using Monopolizers.Service.Contract;

namespace Monopolizers.Service.Implementation
{
    public class AssetService : IAssetService
    {
        private readonly IGenericRepository<Asset> _assetRepo;

        public AssetService(IGenericRepository<Asset> assetRepo)
        {
            _assetRepo = assetRepo;
        }

        public async Task<ResponseDTO> GetAllAssetsAsync()
        {
            var assets = await _assetRepo.GetQueryable().ToListAsync();

            return new ResponseDTO
            {
                IsSucess = true,
                BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY,
                Data = assets.Select(a => new AssetDTO
                {
                    AssetId = a.AssetId,
                    Name = a.Name,
                    AssetType = a.AssetType,
                    SourceUrl = a.SourceUrl,
                    AccessLevel = a.AccessLevel
                })
            };
        }
        }
}
