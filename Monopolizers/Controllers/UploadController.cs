using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Monopolizers.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;

        public UploadController(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadMedia(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided.");

            var ext = Path.GetExtension(file.FileName).ToLower();
            using var stream = file.OpenReadStream();

            if (ext == ".mp4" || ext == ".mov" || ext == ".avi")
            {
                // 👉 Upload video
                var uploadParams = new VideoUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "monopolizers/cards/videos"
                };

                var result = await _cloudinary.UploadAsync(uploadParams);
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Ok(new { url = result.SecureUrl.ToString() });
                }
            }
            else
            {
                // 👉 Upload ảnh (mặc định)
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "monopolizers/cards/images"
                };

                var result = await _cloudinary.UploadAsync(uploadParams);
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Ok(new { url = result.SecureUrl.ToString() });
                }
            }

            return StatusCode(500, "Upload failed.");
        }

    }
}
