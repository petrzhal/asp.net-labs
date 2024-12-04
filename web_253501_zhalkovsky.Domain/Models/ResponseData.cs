using System;

namespace web_253501_zhalkovsky.Domain.Models
{
    public class ResponseData<T>
    {
        public T? Data { get; set; }
        public bool Successful { get; set; } = true;
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Получить объект успешного ответа
        /// </summary>
        /// <param name="data">Передаваемые данные</param>
        /// <returns></returns>
        public static ResponseData<T> Success(T data)
        {
            return new ResponseData<T>
            {
                Data = data,
                Successful = true
            };
        }

        /// <summary>
        /// Получение объекта ответа с ошибкой
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        /// <param name="data">Передаваемые данные</param>
        /// <returns></returns>
        public static ResponseData<T> Error(string message, T? data = default)
        {
            return new ResponseData<T>
            {
                ErrorMessage = message,
                Successful = false,
                Data = data
            };
        }
    }
}
