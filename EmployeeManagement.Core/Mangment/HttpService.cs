using EmployeeManagement.Core.Models;
using System;
using System.Net.Http;
using System.Text.Json;

namespace EmployeeManagement.Core.Mangment
{
    public class HttpService
    {
        private Uri _uri = new Uri("https://localhost:44311/");


        private readonly HttpClient _httpClient;
        private const string Base_Address = "https://localhost:44311/api/";

        public HttpService()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(Base_Address)
            };
        }
        public async Task<T?> HttpGetRequest<T>(string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<T?> HttpGetRequestById<T>(string endpoint, T id)
        {

            string requestUri = $"{endpoint}/{id}";
            return await HttpGetRequest<T>(requestUri);
        }
        public async Task<bool> HttpPostRequest<T>(string requestUri, T entity)
        {
            var jsonString = JsonSerializer.Serialize<T>(entity);
            var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(requestUri, content);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> HttpUpdateRequest<T>(string requestUri, T entity)
        {
            var jsonString = JsonSerializer.Serialize<T>(entity);
            var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(requestUri, content);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        // DeleteCategory
        public async Task<bool> HttpDeleteRequest<T>(string requestUri)
        {
            var response = await _httpClient.DeleteAsync(requestUri);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }


    }
}

