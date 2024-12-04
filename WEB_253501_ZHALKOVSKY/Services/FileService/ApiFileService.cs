using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using web_253501_zhalkovsky.Services.Authentication;
using web_253501_zhalkovsky.Services.FileService;

namespace web_253501_zhalkovsky.Services.FileService
{
    public class ApiFileService : IFileService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenAccessor _tokenAccessor;

        public ApiFileService(ITokenAccessor tokenAccessor, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _tokenAccessor = tokenAccessor;
        }

        public async Task<string> SaveFileAsync(IFormFile formFile)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var extension = Path.GetExtension(formFile.FileName);
            var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);

            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(formFile.OpenReadStream());
            content.Add(streamContent, "file", newName);

            var response = await _httpClient.PostAsync("", content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return string.Empty;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.DeleteAsync($"?fileName={fileName}");
            response.EnsureSuccessStatusCode();
        }
    }
}
