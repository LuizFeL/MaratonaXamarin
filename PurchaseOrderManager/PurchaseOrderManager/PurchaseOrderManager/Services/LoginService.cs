using System;
using System.Text;
using System.Threading.Tasks;
using PurchaseOrderManager.Model;
using PurchaseOrderManager.Storage;

namespace PurchaseOrderManager.Services
{
    public class LoginService
    {
        private const string CriptyKey = "qwertyuiopçlkjhgfdszxcvbnm";


        public static string Encripty(string value)
        {
            var salt = Encoding.UTF8.GetBytes(CriptyKey);
            return BitConverter.ToString(CryptoService.Encrypt(value, CriptyKey, salt));
        }

        public static async Task<Login> FindLogin(string loginName)
        {
            try
            {
                await Task.Delay(50).ConfigureAwait(false);
                var client = new AzureClient();
                var login = await client.GetLogin(loginName);
                return login;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
