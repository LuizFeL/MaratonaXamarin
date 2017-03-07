using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using SQLite;

namespace PurchaseOrderManager.Model
{
    [DataTable("PurchaseOrderItem")]
    public class PurchaseOrderItem : ObservableBaseObject, IKeyObject
    {
        public PurchaseOrderItem()
        {
            Id = Guid.NewGuid().ToString();
        }

        private string _key;
        [PrimaryKey]
        [JsonProperty("Id")]
        public string Id
        {
            get { return _key; }
            set { _key = value; OnPropertyChanged(); }
        }

        private string _pokey;
        [Indexed(Name = "IxPo", Order = 1, Unique = true)]
        [JsonProperty("PoKey")]
        public string PoKey
        {
            get { return _pokey; }
            set { _pokey = value; OnPropertyChanged(); }
        }

        private int _item;
        [Indexed(Name = "IxPo", Order = 2, Unique = true)]
        [JsonProperty("Item")]
        public int Item
        {
            get { return _item; }
            set { _item = value; OnPropertyChanged(); }
        }

        private string _productCode;
        [JsonProperty("ProductCode")]
        public string ProductCode
        {
            get { return _productCode; }
            set { _productCode = value; OnPropertyChanged(); }
        }

        private string _productDesc;
        [JsonProperty("ProductDesc")]
        public string ProductDesc
        {
            get { return _productDesc; }
            set { _productDesc = value; OnPropertyChanged(); }
        }

        private double _qtd;
        [JsonProperty("Qtd")]
        public double Qtd
        {
            get { return _qtd; }
            set { _qtd = value; OnPropertyChanged(); OnPropertyChanged("Total"); OnPropertyChanged("PurchaseOrder"); }
        }

        private double _value;
        [JsonProperty("Value")]
        public double Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(); OnPropertyChanged("Total"); OnPropertyChanged("PurchaseOrder"); }
        }

        [Ignore]
        [JsonIgnore]
        public double Total
        {
            get { return _value * Qtd; }
        }

        private PurchaseOrder _purchaseOrder;
        [Ignore]
        [JsonIgnore]
        public PurchaseOrder PurchaseOrder
        {
            get { return _purchaseOrder; }
            set { _purchaseOrder = value; OnPropertyChanged();  }
        }

        [Ignore]
        [JsonIgnore]
        public string Client => PurchaseOrder?.Client;

        [Ignore]
        [JsonIgnore]
        public string Vendor => PurchaseOrder?.Vendor;

        [Ignore]
        [JsonIgnore]
        public DateTime? Date => PurchaseOrder?.Date;

        private int _index;

        [Ignore]
        [JsonIgnore]
        public int Index
        {
            get { return _index; }
            set { _index = value; OnPropertyChanged(); }
        }

        [Version]
        public string Version { get; set; }
    }
}
