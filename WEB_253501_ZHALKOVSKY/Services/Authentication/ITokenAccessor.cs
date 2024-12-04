﻿namespace web_253501_zhalkovsky.Services.Authentication
{
    public interface ITokenAccessor
    {
        /// <summary>
        /// Получение access-token
        /// </summary>
        /// <returns></returns>
        Task<string> GetAccessTokenAsync();
        /// <summary>
        /// Добавление заголовка Authorizition : bearer
        /// </summary>
        /// <param name="httpClient">HttpLient, в который добавляется заголовок</param>
        /// <returns></returns>
        Task SetAuthorizationHeaderAsync(HttpClient httpClient);
    }

}
