using System;
using System.Linq;
using System.Threading.Tasks;
using PurchaseOrderManager.Model;
using PurchaseOrderManager.ViewModel;
using Xamarin.Forms;

namespace PurchaseOrderManager.Pages
{
    public partial class PurchaseOrderItemPage
    {
        private readonly PurchaseOrderVM _purchaseOrderVM;
        private readonly PurchaseOrder _purchaseOrder;
        private readonly PurchaseOrderItem _purchaseOrderItem;

        public PurchaseOrderItemPage(PurchaseOrderVM vm, PurchaseOrder po, PurchaseOrderItem item)
        {
            InitializeComponent();
            Indicator.WidthRequest = Device.OS == TargetPlatform.Windows ? 200 : 80;
            _purchaseOrderVM = vm;
            _purchaseOrderItem = item;
            _purchaseOrder = po;
            Indicator.IsRunning = Indicator.IsVisible = false;
            BindingContext = item;
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() => Indicator.IsRunning = Indicator.IsVisible = true);
                await Task.Delay(50).ConfigureAwait(false);
                await _purchaseOrderVM.Save(_purchaseOrderItem);
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (_purchaseOrder.Items.All(x => x.Id != _purchaseOrderItem.Id))
                    {
                        _purchaseOrderItem.Index = _purchaseOrder.ItemsCount;
                        _purchaseOrder.Items.Add(_purchaseOrderItem);
                    }
                    _purchaseOrder.OnPropertyChanged("ItemsCount");
                    _purchaseOrder.OnPropertyChanged("ItemsTotal");
                    DisplayAlert("Sucesso", "Item Salvo!", "OK");
                });
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

        private async void Button2_OnClicked(object sender, EventArgs e)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() => Indicator.IsRunning = Indicator.IsVisible = true);
                await Task.Delay(50).ConfigureAwait(false);
                await _purchaseOrderVM.Delete(_purchaseOrderItem);
                Device.BeginInvokeOnMainThread(() =>
                {
                    _purchaseOrder.Items.Remove(_purchaseOrderItem);
                    var index = 0;
                    foreach (var item in _purchaseOrder.Items)
                    {
                        item.Index = index;
                        index++;
                    }
                    _purchaseOrder.OnPropertyChanged("ItemsCount");
                    _purchaseOrder.OnPropertyChanged("ItemsTotal");
                    DisplayAlert("Sucesso", "Item Excluído!", "OK");
                    Navigation.PopAsync();
                });
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

        private void PurchaseOrderItemPage_OnAppearing(object sender, EventArgs e)
        {
            if (ToolbarItems.Any(x => x.Text == App.CurrentUser.Id)) return;
            ToolbarItems.Add(new ToolbarItem
            {
                Icon = "usericon.png",
                Text = App.CurrentUser.Id,
                Command = new Command(() => DisplayAlert("Login info", App.CurrentUser.Id, "OK"))
            });
        }
    }
}
