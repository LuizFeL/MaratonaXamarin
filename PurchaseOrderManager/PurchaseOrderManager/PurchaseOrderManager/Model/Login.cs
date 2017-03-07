using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using SQLite;

namespace PurchaseOrderManager.Model
{
    [DataTable("Login")]
    public class Login : ObservableBaseObject, IKeyObject
    {
        private string _key;
        [PrimaryKey]
        [JsonProperty("Id")]
        public string Id
        {
            get { return _key; }
            set { _key = value; OnPropertyChanged(); }
        }
        
        private string _password;
        [JsonProperty("Password")]
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        [Version]
        public string Version { get; set; }

    }
}
