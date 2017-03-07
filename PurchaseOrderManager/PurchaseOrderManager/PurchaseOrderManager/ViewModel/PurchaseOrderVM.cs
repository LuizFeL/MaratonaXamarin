using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using PurchaseOrderManager.Model;
using PurchaseOrderManager.Services;
using Xamarin.Forms;

namespace PurchaseOrderManager.ViewModel
{
    public class PurchaseOrderVM : ObservableBaseObject
    {
        public PurchaseOrderVM()
        {
            PurchaseOrders = new ObservableCollection<PurchaseOrder>();
            LoadDirectoryCommand = new Command(LoadDirectory, () => !IsBusy);
            IsBusy = false;
        }

        public ObservableCollection<PurchaseOrder> PurchaseOrders { get; set; }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public bool IsNotBusy => !_isBusy;

        public Command LoadDirectoryCommand { get; set; }

        public async void LoadDirectory()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                PurchaseOrders.Clear();
                await Task.Delay(100).ConfigureAwait(false);
                var directory = await PurchaseOrderDirectoryService.LoadPurchaseOrderDirectory();
                var index = 0;
                Device.BeginInvokeOnMainThread(() =>
                {
                    foreach (var purchaseOrder in directory.PurchaseOrders)
                    {
                        purchaseOrder.Index = index;
                        PurchaseOrders.Add(purchaseOrder);
                        index++;
                    }
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task Save(PurchaseOrder po)
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                await Task.Delay(100).ConfigureAwait(false);
                await PurchaseOrderDirectoryService.Save(po);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task Save(PurchaseOrderItem item)
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                await Task.Delay(100).ConfigureAwait(false);
                await PurchaseOrderDirectoryService.Save(item);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task Delete(PurchaseOrderItem item)
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                await Task.Delay(100).ConfigureAwait(false);
                await PurchaseOrderDirectoryService.Delete(item);
            }
            finally
            {
                IsBusy = false;
            }
        }

    }


    public class ListKeyBacgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var iValue = (int)value;
                return (iValue % 2) != 0 ? Color.Transparent : Color.FromHex("#3399ff").MultiplyAlpha(0.2);
            }
            catch
            {
                return Color.Transparent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null; //Do nothing
        }
    }
}
