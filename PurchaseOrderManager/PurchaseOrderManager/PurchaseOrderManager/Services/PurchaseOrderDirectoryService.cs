using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PurchaseOrderManager.Model;

namespace PurchaseOrderManager.Services
{
    public class PurchaseOrderDirectoryService
    {
        public static async Task<PurchaseOrderDirectory> LoadPurchaseOrderDirectory()
        {
            await Task.Delay(50).ConfigureAwait(false);
            var directory = new PurchaseOrderDirectory();
            var azureClient = new AzureClient();

            var poList = await azureClient.GetPOs(true);

            await azureClient.SyncPoItemsAsync();

            var purchaseOrders = poList as PurchaseOrder[] ?? poList.ToArray();

            foreach (var purchaseOrder in purchaseOrders)
            {
                var poKey = purchaseOrder.Id;
                purchaseOrder.Items = new ObservableCollection<PurchaseOrderItem>(await azureClient.GetPoItems(poKey));
                var index = 0;
                foreach (var item in purchaseOrder.Items)
                {
                    item.Index = index;
                    item.PurchaseOrder = purchaseOrder;
                    index++;
                }
            }

            directory.PurchaseOrders = new ObservableCollection<PurchaseOrder>(purchaseOrders);
            return directory;
        }

        public static async Task Save(PurchaseOrder purchaseOrder)
        {
            await Task.Delay(50).ConfigureAwait(false);
            new AzureClient().Save(purchaseOrder);
        }

        public static async Task Save(PurchaseOrderItem item)
        {
            await Task.Delay(50).ConfigureAwait(false);
            new AzureClient().Save(item);
        }

        public static async Task Delete(PurchaseOrderItem item)
        {
            await Task.Delay(50).ConfigureAwait(false);
            new AzureClient().Delete(item);
        }
    }
}
