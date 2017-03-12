using System;
using System.Linq;
using System.Threading.Tasks;
using PurchaseOrderManager.Model;
using PurchaseOrderManager.ViewModel;
using Xamarin.Forms;

namespace PurchaseOrderManager.Pages
{
    public partial class PurchaseOrderPage
    {
        private readonly PurchaseOrder _purchaseOrder;
        private readonly PurchaseOrderVM _purchaseOrderVM;

        public PurchaseOrderPage(PurchaseOrder po, PurchaseOrderVM vm)
        {
            try
            {

                InitializeComponent();
                Indicator.WidthRequest = Device.OS == TargetPlatform.Windows ? 200 : 80;
                BindingContext = po;
                _purchaseOrder = po;
                _purchaseOrderVM = vm;
                Indicator.IsRunning = Indicator.IsVisible = false;
                PoItemListView.ItemSelected += (sender, args) =>
                {
                    var selectedItem = PoItemListView.SelectedItem as PurchaseOrderItem;
                    if (selectedItem == null) return;
                    Navigation.PushAsync(new PurchaseOrderItemPage(_purchaseOrderVM, _purchaseOrder, selectedItem));
                    PoItemListView.SelectedItem = null;
                };
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() => Indicator.IsRunning = Indicator.IsVisible = true);
                await Task.Delay(50).ConfigureAwait(false);
                await _purchaseOrderVM.Save(_purchaseOrder);
                Device.BeginInvokeOnMainThread(() => DisplayAlert("Sucesso", "Pedido Salvo!", "OK"));
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() => DisplayAlert("Erro", ex.Message, "OK"));
            }
            finally
            {
                Device.BeginInvokeOnMainThread(() => Indicator.IsRunning = Indicator.IsVisible = false);
            }
        }

        private void Button2_OnClicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new PurchaseOrderItemPage(_purchaseOrderVM, _purchaseOrder,
                    new PurchaseOrderItem
                    {
                        Item = _purchaseOrder.GetLastItem(),
                        PoKey = _purchaseOrder.Id,
                        PurchaseOrder = _purchaseOrder
                    }));
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void PurchaseOrderPage_OnAppearing(object sender, EventArgs e)
        {
            try
            {
                if (ToolbarItems.Any(x => x.Text == App.CurrentUser.Id)) return;
                ToolbarItems.Add(new ToolbarItem
                {
                    Icon = "usericon.png",
                    Text = App.CurrentUser.Id,
                    Command = new Command(() => DisplayAlert("Login info", App.CurrentUser.Id, "OK"))
                });
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
