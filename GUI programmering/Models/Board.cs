using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GUI_programmering.Models
{
    public class Board : INotifyPropertyChanged
    {
        private ObservableCollection<Product> _ProductsForSale;

        public ObservableCollection<Product> ProductsForSale
        {
            get { return _ProductsForSale;} set { _ProductsForSale = value; OnPropertyChanged("ProductsForSale"); } }

        private ObservableCollection<Product> _ProductsToBuy;

        public ObservableCollection<Product> ProductsToBuy
        {
            get { return _ProductsToBuy; }
            set { _ProductsToBuy = value; OnPropertyChanged("ProductsToBuy"); }
        }

        public Board(ObservableCollection<Product> productsForSale, ObservableCollection<Product> productsToBuy)
        {
            ProductsForSale = productsForSale;
            ProductsToBuy = productsToBuy;
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
