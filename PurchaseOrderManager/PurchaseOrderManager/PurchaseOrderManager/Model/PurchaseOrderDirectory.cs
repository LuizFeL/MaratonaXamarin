using System.Collections.ObjectModel;

namespace PurchaseOrderManager.Model
{
    public class PurchaseOrderDirectory : ObservableBaseObject
    {
        public PurchaseOrderDirectory()
        {
            PurchaseOrders = new ObservableCollection<PurchaseOrder>();
        }
        
        public ObservableCollection<PurchaseOrder> PurchaseOrders { get; set; }
        public int OrdersCount => PurchaseOrders?.Count ?? 0;
    }
}
