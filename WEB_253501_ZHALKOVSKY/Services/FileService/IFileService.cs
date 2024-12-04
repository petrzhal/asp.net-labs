namespace web_253501_zhalkovsky.Services.FileService
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface IFileService
    {
        /// <summary>
        /// Сохранить файл
        /// </summary>
        /// <param name="formFile">Файл, переданный формой</param>
        /// <returns>URL сохраненного файла</returns>
        Task<string> SaveFileAsync(IFormFile formFile);

        /// <summary>
        /// Удалить файл
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        Task DeleteFileAsync(string fileName);
    }

}
