using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using PurchaseOrderManager.Model;

namespace PurchaseOrderManager.Services
{
    public class AzureClient
    {
        private readonly IMobileServiceClient _client;
        private readonly IMobileServiceSyncTable<Login> _loginTable;
        private readonly IMobileServiceSyncTable<PurchaseOrder> _purchaseOrderTable;
        private readonly IMobileServiceSyncTable<PurchaseOrderItem> _purchaseOrderItemTable;
        const string DbPath = "data.db";
        private const string ServiceUri = "http://maratonaxamarinpom.azurewebsites.net/";
        
        public AzureClient()
        {
            _client = new MobileServiceClient(ServiceUri);
            var store = new MobileServiceSQLiteStore(DbPath);
            store.DefineTable<Login>();
            store.DefineTable<PurchaseOrder>();
            store.DefineTable<PurchaseOrderItem>();
            _client.SyncContext.InitializeAsync(store);
            _loginTable = _client.GetSyncTable<Login>();
            _purchaseOrderTable = _client.GetSyncTable<PurchaseOrder>();
            _purchaseOrderItemTable = _client.GetSyncTable<PurchaseOrderItem>();
#if DEBUG
            Save(new Login { Id = "LuizFel", Password = LoginService.Encripty("123") });
#endif
        }



        public async Task<IEnumerable<Login>> GetLogins()
        {
            var empty = new Login[0];
            try
            {
                if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                    await SyncPOsAsync();

                return await _loginTable.ToEnumerableAsync();
            }
            catch (Exception)
            {
                return empty;
            }
        }

        public async Task<IEnumerable<PurchaseOrderItem>> GetPoItems()
        {
            var empty = new PurchaseOrderItem[0];
            try
            {
                if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                    await SyncPOsAsync();

                return await _purchaseOrderItemTable.ToEnumerableAsync();
            }
            catch (Exception)
            {
                return empty;
            }
        }

        public async Task<IEnumerable<PurchaseOrder>> GetPOs()
        {
            var empty = new PurchaseOrder[0];
            try
            {
                if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                    await SyncPOsAsync();

                return await _purchaseOrderTable.ToEnumerableAsync();
            }
            catch (Exception)
            {
                return empty;
            }
        }



        public async Task<Login> GetLogin(string key)
        {
            try
            {
                var queryResult = await _loginTable.Where(x => x.Id == key).ToListAsync();
                return queryResult.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<PurchaseOrder> GetPo(string key)
        {
            try
            {
                var queryResult = await _purchaseOrderTable.Where(x => x.Id == key).ToListAsync();
                return queryResult.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<PurchaseOrderItem> GetPoItem(string key)
        {
            try
            {
                var queryResult = await _purchaseOrderItemTable.Where(x => x.Id == key).ToListAsync();
                return queryResult.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<PurchaseOrderItem>> GetPoItems(string poKey)
        {
            try
            {
                var queryResult = await _purchaseOrderItemTable.Where(x => x.PoKey == poKey).ToListAsync();
                return queryResult.ToList();
            }
            catch (Exception)
            {
                return new PurchaseOrderItem[0];
            }
        }



        public async void Save(Login data)
        {
            var table = _loginTable;

            if (await GetLogin(data.Id) == null)
            {
                await table.InsertAsync(data);
                return;
            }

            await table.UpdateAsync(data);
        }

        public async void Save(PurchaseOrder data)
        {
            var table = _purchaseOrderTable;

            if (await GetPo(data.Id) == null)
            {
                await table.InsertAsync(data);
                return;
            }

            await table.UpdateAsync(data);
        }

        public async void Save(PurchaseOrderItem data)
        {
            var table = _purchaseOrderItemTable;

            if (await GetPoItem(data.Id) == null)
            {
                await table.InsertAsync(data);
                return;
            }

            await table.UpdateAsync(data);
        }



        public async Task SyncLoginsAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
            try
            {
                await _client.SyncContext.PushAsync();

                await _loginTable.PullAsync("allLogins", _loginTable.CreateQuery());
            }
            catch (MobileServicePushFailedException pushEx)
            {
                if (pushEx.PushResult != null)
                    syncErrors = pushEx.PushResult.Errors;
            }
        }

        public async Task SyncPOsAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
            try
            {
                await _client.SyncContext.PushAsync();

                await _purchaseOrderTable.PullAsync("allPOs", _purchaseOrderTable.CreateQuery());
            }
            catch (MobileServicePushFailedException pushEx)
            {
                if (pushEx.PushResult != null)
                    syncErrors = pushEx.PushResult.Errors;
            }
        }

        public async Task SyncPoItemsAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
            try
            {
                await _client.SyncContext.PushAsync();

                await _purchaseOrderItemTable.PullAsync("allPoItems", _purchaseOrderItemTable.CreateQuery());
            }
            catch (MobileServicePushFailedException pushEx)
            {
                if (pushEx.PushResult != null)
                    syncErrors = pushEx.PushResult.Errors;
            }
        }



        public async Task CleanPoData()
        {
            await _purchaseOrderTable.PurgeAsync("allPOs", _purchaseOrderTable.CreateQuery(), new System.Threading.CancellationToken());
        }

        public async Task CleanPoItemData()
        {
            await _purchaseOrderItemTable.PurgeAsync("allPoItems", _purchaseOrderItemTable.CreateQuery(), new System.Threading.CancellationToken());
        }

        public async Task CleanLoginData()
        {
            await _purchaseOrderTable.PurgeAsync("allLogins", _loginTable.CreateQuery(), new System.Threading.CancellationToken());
        }



        public async void Delete(PurchaseOrderItem data)
        {
            var table = _purchaseOrderItemTable;
            if (await GetPoItem(data.Id) == null) return;
            await table.DeleteAsync(data);
        }

    }

}
