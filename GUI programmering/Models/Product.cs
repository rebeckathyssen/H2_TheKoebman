using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_programmering
{
    public class Product
    {
        public string ProductType { get; set; }
        public int SellsFor { get; set; }
        public int BuyersPrice { get; set; }
        public bool WantToSell { get; set; } // marks if the item is for sale
        public bool WantToBuy { get; set; } // marks if the item can be sold
        public Product(string productType)
        {
            ProductType = productType;
            SellsFor = 0;
            BuyersPrice = 0;
            WantToSell = false;
            WantToBuy = false;
        }
    }
}
