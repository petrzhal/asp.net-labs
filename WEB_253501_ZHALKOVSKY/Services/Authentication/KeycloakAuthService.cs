using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;
using web_253501_zhalkovsky.HelperClasses;
using web_253501_zhalkovsky.Services.FileService;
using web_253501_zhalkovsky.Models;

namespace web_253501_zhalkovsky.Services.Authentication
{
    public class KeycloakAuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IFileService _fileService;
        private readonly ITokenAccessor _tokenAccessor;
        KeycloakData _keycloakData;

        public KeycloakAuthService(HttpClient httpClient,
                                   IOptions<KeycloakData> options,
                                   IFileService fileService,
                                   ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _fileService = fileService;
            _tokenAccessor = tokenAccessor;
            _keycloakData = options.Value;
        }

        public async Task<(bool Result, string ErrorMessage)> RegisterUserAsync(
            string email, string password, IFormFile? avatar)
        {
            try
            {
                await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

            var avatarUrl = "/images/default-profile-picture.png";

            if (avatar != null)
            {
                var result = await _fileService.SaveFileAsync(avatar);
                if (result != null) avatarUrl = result;
            }

            var newUser = new CreateUserModel
            {
                Username = email,
                Email = email,
                Enabled = true,
                EmailVerified = true,
                Attributes = new Dictionary<string, string> { { "avatar", avatarUrl } },
                Credentials = new List<UserCredentials>
            {
                new UserCredentials
                {
                    Type = "password",
                    Value = password,
                    Temporary = false
                }
            }
            };

            var requestUri = $"{_keycloakData.Host}/admin/realms/{_keycloakData.Realm}/users";

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var userData = JsonSerializer.Serialize(newUser, serializerOptions);
            HttpContent content = new StringContent(userData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(requestUri, content);
            if (response.IsSuccessStatusCode)
            {
                return (true, String.Empty);
            }

            return (false, response.StatusCode.ToString());
        }
    }
}
