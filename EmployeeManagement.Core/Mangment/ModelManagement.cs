using System.Security.Cryptography;
using System.Text;


namespace EmployeeManagement.Core.Mangment
{
    public class ModelManagement
    {
        private readonly HttpService _httpService;
        public ModelManagement(HttpService httpService)
        {
            _httpService = httpService;
        }



        //Updatera listan i db
        public async Task<List<T>?> UpdateListAsync<T>() where T : class 
        {
            return await _httpService.HttpGetRequest<List<T>>(typeof(T).Name);
        }

        public async Task<bool> CheckIfEntityExistsInDBAsync<T>() // Kolla om det finns entity i db
    where T : class
        {
            return (await UpdateListAsync<T>()).Any();
        }


        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512; // Ange vilken hashalgoritm som ska användas

        public string HashPassword(string password)// Metod för att hasha lösenord
        {
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                Array.Empty<byte>(),
                iterations,
                hashAlgorithm,
                keySize);

            return Convert.ToHexString(hash);
        }

        public bool VerifyPassword(string password, string hash)// Metod för att verifiera ett lösenord
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, Array.Empty<byte>(), iterations, hashAlgorithm, keySize);

            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }
    }
}
