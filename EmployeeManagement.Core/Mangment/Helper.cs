using EmployeeManagement.Core.Models;
using System.ComponentModel.Design;

namespace EmployeeManagement.Core.Mangment
{
    public class Helper
    {
        private static HttpService _httpService;

        public static void Configure(HttpService httpService)
        {
            _httpService = httpService;
        }
        public static async Task<byte[]> UploadImageAsync(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                await using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    return dataStream.ToArray();
                }
            }
            return null;
        }
    }
}
