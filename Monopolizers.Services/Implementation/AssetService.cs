using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using Monopolizers.Common.BusinessCode;
using Monopolizers.Common.DTO;
using Monopolizers.Common.DTO.Request;
using Monopolizers.Repository.Contract;
using Monopolizers.Repository.DB;
using Monopolizers.Service.Contract;

namespace Monopolizers.Service.Implementation
{
    public class AssetService : IAssetService
    {
        private readonly IGenericRepository<Asset> _assetRepo;
        private readonly Cloudinary _cloudinary;
        private readonly IUnitOfWork _unitOfWork;

        public AssetService(IGenericRepository<Asset> assetRepo, Cloudinary cloudinary, IUnitOfWork unitOfWork)
        {
            _assetRepo = assetRepo;
            _cloudinary = cloudinary;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> BulkImportAssetsAsync(BulkAssetUploadRequestDTO dto, string userId)
        {
            var uploadedAssets = new List<Asset>();

            foreach (var item in dto.Assets)
            {
                using var stream = item.File.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(item.File.FileName, stream),
                    Folder = "monopolizers/assets"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                    continue;

                var asset = new Asset
                {
                    Name = item.Name,
                    AssetType = item.AssetType,
                    AccessLevel = item.AccessLevel,
                    SourceUrl = uploadResult.SecureUrl.ToString(),
                    UploadedAt = DateTime.UtcNow,
                    UserId = userId
                };

                uploadedAssets.Add(asset);
            }

            await _assetRepo.InsertRange(uploadedAssets);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO
            {
                IsSucess = true,
                BusinessCode = BusinessCode.CREATE_SUCCESS,
                message = $"{uploadedAssets.Count} assets uploaded successfully."
            };
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
