using System;
using System.Linq;
using System.Threading.Tasks;
using PurchaseOrderManager.Model;
using PurchaseOrderManager.Services;
using Xamarin.Forms;

namespace PurchaseOrderManager.Pages
{
    public partial class LoginPage
    {
        private readonly ToolbarItem _azureSyncToolbarItem = new ToolbarItem
        {
            Icon = "azure_on.png",
            Text = "Status: ONLINE"
        };

        private bool _isExec;

        public bool IsExecuting
        {
            get
            {
                return _isExec;
            }
            set
            {
                _isExec = value;
                OnPropertyChanged();
                Indicator.IsRunning = value;
                Indicator.IsVisible = value;
            }
        }

        public LoginPage()
        {
            InitializeComponent();
            Indicator.WidthRequest = Device.OS == TargetPlatform.Windows ? 200 : 80;
            IsExecuting = false;
        }

        private void Login_OnAppearing(object sender, EventArgs e)
        {
            IsExecuting = false;

#if DEBUG
            LoginEntry.Text = "LuizFel";
            PwdEntry.Text = "123";
#endif

            App.CurrentUser = null;
            UpdateLoginStatus();
            if (ToolbarItems.All(x => !x.Text.StartsWith("Status: "))) ToolbarItems.Add(_azureSyncToolbarItem);

            if (ToolbarItems.Any(x => x.Text == "Atualizar Logins")) return;
            ToolbarItems.Add(new ToolbarItem
            {
                Icon = "refresh.png",
                Text = "Atualizar Logins",
                Command = new Command(async () =>
                {
                    IsExecuting = true;
                    await new AzureClient().GetLogins();
                    UpdateLoginStatus();
                    IsExecuting = false;
                })
            });
        }

        private void UpdateLoginStatus()
        {
            _azureSyncToolbarItem.Icon = AzureClient.LoginSync ? "azure_on.png" : "azure_off.png";
            _azureSyncToolbarItem.Text = "Status: " + (AzureClient.LoginSync ? "ONLINE" : "OFFLINE");
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            try
            {
                App.CurrentUser = null;
                if (string.IsNullOrWhiteSpace(LoginEntry.Text))
                {
                    await DisplayAlert("Error", "Preencha o login", "OK");
                    LoginEntry.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(PwdEntry.Text))
                {
                    await DisplayAlert("Error", "Preencha a senha", "OK");
                    PwdEntry.Focus();
                    return;
                }

                var isLogged = await CheckLogin(LoginEntry.Text, PwdEntry.Text);
                if (!isLogged) return;

                await Navigation.PushAsync(new PurchaseOrderListPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task<bool> CheckLogin(string name, string pwd)
        {
            try
            {
                IsExecuting = true;
                await Task.Delay(150);
                var login = await LoginService.FindLogin(name);

                if (login == null)
                {
                    if (!AzureClient.LoginSync) //Only for testing
                    {
                        App.CurrentUser = new Login { Id = "Test", Password = "" };
                        return true;
                    }

                    Device.BeginInvokeOnMainThread(() => DisplayAlert("Error", "Login inválido", "OK"));
                    return false;
                }

                var sPwd = LoginService.Encripty(pwd);
                if (login.Password == sPwd)
                {
                    App.CurrentUser = login;
                    return true;
                }

                Device.BeginInvokeOnMainThread(() => DisplayAlert("Error", "Senha inválida", "OK"));
                return false;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
            finally
            {
                IsExecuting = false;
            }
        }
    }
}
