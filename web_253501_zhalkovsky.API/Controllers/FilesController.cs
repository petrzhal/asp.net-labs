using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace web_253501_zhalkovsky.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly string _imagePath;

        public FilesController(IWebHostEnvironment webHost)
        {
            _imagePath = Path.Combine(webHost.WebRootPath, "Images");
        }

        [HttpPost]
        public async Task<IActionResult> SaveFile(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("File is null.");
            }

            var filePath = Path.Combine(_imagePath, file.FileName);
            var fileInfo = new FileInfo(filePath);

            // Удаление существующего файла, если есть
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }

            // Сохранение файла
            using var fileStream = fileInfo.Create();
            await file.CopyToAsync(fileStream);

            // Получение URL файла
            var host = HttpContext.Request.Host;
            var fileUrl = $"https://{host}/Images/{file.FileName}";

            return Ok(fileUrl);
        }

        [HttpDelete]
        public IActionResult DeleteFile(string fileName)
        {
            var filePath = Path.Combine(_imagePath, fileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return Ok($"File {fileName} deleted successfully.");
            }

            return NotFound("File not found.");
        }
    }
}
