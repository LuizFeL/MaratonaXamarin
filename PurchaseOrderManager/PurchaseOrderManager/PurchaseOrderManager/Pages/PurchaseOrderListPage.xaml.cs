using System;
using System.Linq;
using PurchaseOrderManager.Model;
using PurchaseOrderManager.ViewModel;
using Xamarin.Forms;

namespace PurchaseOrderManager.Pages
{
    public partial class PurchaseOrderListPage
    {
        public PurchaseOrderListPage()
        {
            InitializeComponent();
            Indicator.WidthRequest = Device.OS == TargetPlatform.Windows ? 200 : 80;
            BindingContext = new PurchaseOrderVM();
            RefreshButton.IsVisible = Device.OS == TargetPlatform.Windows;
        }

        private void PurchaseOrderListPage_OnAppearing(object sender, EventArgs e)
        {
            var vm = (BindingContext as PurchaseOrderVM);
            vm?.LoadDirectory();

            PoListView.ItemSelected += (o, args) =>
            {
                var poSelected = (PurchaseOrder)PoListView.SelectedItem;
                if (poSelected == null) return;
                Navigation.PushAsync(new PurchaseOrderPage(poSelected, vm));
                PoListView.SelectedItem = null;
            };

            if (ToolbarItems.Any(x => x.Text == App.CurrentUser.Id)) return;

            ToolbarItems.Add(new ToolbarItem
            {
                Icon = "usericon.png",
                Text = App.CurrentUser.Id,
                Command = new Command(() => DisplayAlert("Login info", App.CurrentUser.Id, "OK"))
            });
        }

        private void ButtonNew_OnClicked(object sender, EventArgs e)
        {
            var vm = (BindingContext as PurchaseOrderVM);
            vm?.LoadDirectory();
            Navigation.PushAsync(new PurchaseOrderPage(new PurchaseOrder { Index = vm?.PurchaseOrders.Count ?? 1 }, vm));
        }
    }
}
