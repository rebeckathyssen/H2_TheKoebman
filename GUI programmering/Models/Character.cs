using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Net.Mime;
using System.Windows.Media.Imaging;

namespace GUI_programmering
{
    public class Character : INotifyPropertyChanged
    {
        private string _LastVisitedLocation;

        public string LastVisitedLocation
        {
            get { return _LastVisitedLocation; }
            set { _LastVisitedLocation = value; OnPropertyChanged("LastVisitedLocation"); }
        }

        private ObservableCollection<Product> _Inventory;
        public ObservableCollection<Product> Inventory
        {
            get { return _Inventory;}
            set { _Inventory = value; OnPropertyChanged("Inventory"); }
        }

        private int _Currency;
        public int Currency
        {
            get { return _Currency;}
            set { _Currency = value; OnPropertyChanged("Currency"); }
        }

        private int _LifeTime;
        public int LifeTime
        {
            get { return _LifeTime;}
            set { _LifeTime = value; OnPropertyChanged("LifeTime"); }
        }

        public Character(ObservableCollection<Product> inventory, int currency)
        {
            Inventory = inventory;

            Currency = currency;

            LifeTime = 0;

            LastVisitedLocation = "No where";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
