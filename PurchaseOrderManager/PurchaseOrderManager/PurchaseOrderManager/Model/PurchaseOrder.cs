using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using SQLite;

namespace PurchaseOrderManager.Model
{
    [DataTable("PurchaseOrder")]
    public class PurchaseOrder : ObservableBaseObject, IKeyObject
    {
        public PurchaseOrder()
        {
            Items = new ObservableCollection<PurchaseOrderItem>();
            Date = DateTime.Now;
            Id = Guid.NewGuid().ToString();
        }

        public int GetLastItem()
        {
            return ItemsCount == 0 ? 1 : Items.Max(x => x.Item) + 1;
        }

        private ObservableCollection<PurchaseOrderItem> _items;
        [Ignore]
        [JsonIgnore]
        public ObservableCollection<PurchaseOrderItem> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged("ItemsCount");
                OnPropertyChanged("ItemsTotal");
            }
        }

        [Ignore]
        [JsonIgnore]
        public int ItemsCount => Items?.Count ?? 0;

        [Ignore]
        [JsonIgnore]
        public double ItemsTotal => (Items?.Count ?? 0) == 0 ? 0 : Items.Sum(x => x.Total);

        private string _key;
        [PrimaryKey]
        [JsonProperty("Id")]
        public string Id
        {
            get { return _key; }
            set { _key = value; OnPropertyChanged(); }
        }

        private int _index;
        [Ignore]
        [JsonIgnore]
        public int Index
        {
            get { return _index; }
            set { _index = value; OnPropertyChanged(); }
        }

        private string _client;
        [JsonProperty("Client")]
        public string Client
        {
            get { return _client; }
            set { _client = value; OnPropertyChanged(); }
        }

        private string _vendor;
        [JsonProperty("Vendor")]
        public string Vendor
        {
            get { return _vendor; }
            set { _vendor = value; OnPropertyChanged(); }
        }

        private DateTime _date;
        [JsonProperty("Date")]
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(); }
        }

        [Version]
        public string Version { get; set; }
    }
}
