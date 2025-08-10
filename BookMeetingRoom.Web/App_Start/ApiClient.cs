using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BookMeetingRoom.Web.App_Start
{
    public static class ApiClient
    {
        private static readonly HttpClient _httpClient;

        static ApiClient()
        {
            string baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static HttpClient Instance => _httpClient;
    }

}