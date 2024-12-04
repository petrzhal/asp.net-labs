using System.Configuration;
using web_253501_zhalkovsky.Services.CategoryService;
using web_253501_zhalkovsky.Services.FileService;
using web_253501_zhalkovsky.Services.ProductService;
using web_253501_zhalkovsky.HelperClasses;
using web_253501_zhalkovsky.Services.Authentication;
using web_253501_zhalkovsky.Domain.Models;
using web_253501_zhalkovsky.Services;

namespace web_253501_zhalkovsky.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoryService, ApiCategoryService>();
            builder.Services.AddScoped<IProductService, ApiProductService>();
            builder.Services.Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));
            builder.Services.AddHttpClient<ITokenAccessor, KeycloakTokenAccessor>();
            builder.Services.AddHttpClient<IAuthService, KeycloakAuthService>();
        }
        public static void AddFileService(this IServiceCollection services, Uri apiUri)
        {
            services.AddHttpClient<IFileService, ApiFileService>(opt =>
            {
                opt.BaseAddress = new Uri($"{apiUri}/api/Files");
            });
        }
    }
}
