using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOrderManager.Model
{
    public class ObservableBaseObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            if (string.IsNullOrWhiteSpace(name) || PropertyChanged == null) return;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
