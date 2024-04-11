using Azure.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;


namespace ShopClothes.WebApi.Controllers
{
    public class UploadController: ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UploadController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile formFile)
        {
            var folderName = "Upload";
            var webRootPath = _webHostEnvironment.WebRootPath;
            var newPath = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (formFile.Length > 0)
            {
                var filePath= Path.Combine(webRootPath, folderName, formFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                await formFile.CopyToAsync(stream);
                }
                return Ok(new {filePath});
            }
            return BadRequest();
        }

   
    }
 }
