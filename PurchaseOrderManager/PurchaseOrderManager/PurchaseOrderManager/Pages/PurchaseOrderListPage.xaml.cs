﻿using System;
using System.Linq;
using PurchaseOrderManager.Model;
using PurchaseOrderManager.Services;
using PurchaseOrderManager.ViewModel;
using Xamarin.Forms;

namespace PurchaseOrderManager.Pages
{
    public partial class PurchaseOrderListPage
    {
        private readonly ToolbarItem _azureSyncToolbarItem = new ToolbarItem
        {
            Icon = "azure_on.png",
            Text = "Status: ONLINE"
        };

        public PurchaseOrderListPage()
        {
            try
            {
                InitializeComponent();
                Indicator.WidthRequest = Device.OS == TargetPlatform.Windows ? 200 : 80;
                BindingContext = new PurchaseOrderVM();
                RefreshButton.IsVisible = Device.OS == TargetPlatform.Windows;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void PurchaseOrderListPage_OnAppearing(object sender, EventArgs e)
        {
            try
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

                if (ToolbarItems.All(x => !x.Text.StartsWith("Status: "))) ToolbarItems.Add(_azureSyncToolbarItem);

                UpdateStatus();
                PoListView.ItemDisappearing += (s, args) => { UpdateStatus(); };

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

        private void UpdateStatus()
        {
            if (_azureSyncToolbarItem == null) return;
            Device.BeginInvokeOnMainThread(() =>
            {
                _azureSyncToolbarItem.Icon = AzureClient.PurchaseOrderSync ? "azure_on.png" : "azure_off.png";
                _azureSyncToolbarItem.Text = "Status: " + (AzureClient.PurchaseOrderSync ? "ONLINE" : "OFFLINE");
            });
        }

        private void ButtonNew_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var vm = (BindingContext as PurchaseOrderVM);
                vm?.LoadDirectory();
                Navigation.PushAsync(new PurchaseOrderPage(new PurchaseOrder { Index = vm?.PurchaseOrders.Count ?? 1 }, vm));
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "OK");
            }
        }

    }
}
