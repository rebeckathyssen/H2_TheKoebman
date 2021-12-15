using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using GUI_programmering.Models;
using GUI_programmering.Views;

namespace GUI_programmering.ViewModels
{
    public class MainWindowViewModel : IValueConverter
    {
        public Character Character { get; set; }

        public Board Board { get; set; }

        Random random = new Random();

        private string conString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=\"GUI programmering\";Integrated Security=True";

        public MainWindowViewModel()
        {
            Character = new Character(ProductsToList(), 50);

            Board = new Board(GenerateProductsForSale(), GenerateProductsForBuying());
        }

        private ObservableCollection<Product> ProductsToList()
        {
            // Items for the character to start with
            ObservableCollection<Product> products = new ObservableCollection<Product>();
            Product cabbage = new Product("cabbage");
            Product strawberry = new Product("strawberry");
            Product fish = new Product("fish");
            Product egg = new Product("egg");
            Product wheat = new Product("wheat");
            products.Add(cabbage);
            products.Add(strawberry);
            products.Add(fish);
            products.Add(egg);
            products.Add(wheat);
            products.Add(cabbage);
            return products;
        }

        private ObservableCollection<Product> GenerateProductsForSale()
        {
            ObservableCollection<Product> productsForSale = new ObservableCollection<Product>();
            //Generate list of 5 random products to sell on the board
            for (int i = 0; i < 5; i++)
            {
                productsForSale.Add(GenerateRandomProduct());
            }

            // Set price to something random and mark that it's an item for sale
            foreach (Product item in productsForSale)
            {
                item.SellsFor = random.Next(10, 50);
                item.WantToSell = true;
            }
            return productsForSale;
        }

        private ObservableCollection<Product> GenerateProductsForBuying()
        {
            ObservableCollection<Product> productsForBuying = new ObservableCollection<Product>();
            for (int i = 0; i < 5; i++)
            {
                productsForBuying.Add(GenerateRandomProduct());
            }

            foreach (Product item in productsForBuying)
            {
                item.BuyersPrice = random.Next(10, 50);
                item.WantToBuy = true;
            }
            return productsForBuying;
        }

        private Product GenerateRandomProduct()
        {
            // Returns a random string from the list below
            var list = new List<string> { "wheat", "egg", "fish", "strawberry", "cabbage" };
            var list2 = new ObservableCollection<string> { "wheat", "egg", "fish", "strawberry", "cabbage" };
            Product randomProduct = new Product(list2[random.Next(0, list2.Count())]);
            return randomProduct;
        }

        /// DB example ///
        public void GrabTheClickedBuilding(string building)
        {
            Character.LastVisitedLocation = building;

            // Post the characters new status to the database
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            if (con.State == System.Data.ConnectionState.Open)
            {
                string q = "Insert into CharLocation(location)values('" + Character.LastVisitedLocation + "')";
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.ExecuteNonQuery();
                //MessageBox.Show("Connection was succesful!");

                // Get the characters latest status from the database
                string queryString =
                    "SELECT Location FROM CharLocation";

                using (con)
                {
                    SqlCommand command = new SqlCommand(queryString, con);

                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Character.LastVisitedLocation = reader.GetString(0);
                            Console.WriteLine("\t{0}",
                                reader[0]);
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    Console.ReadLine();
                }
            }
            else
            {
                MessageBox.Show("Could not store location in DB!");
            }

            con.Close();

            // Generate new products for the board
            Board.ProductsForSale = GenerateProductsForSale();
            
            Board.ProductsToBuy = GenerateProductsForBuying();
            
            // Add +1 to character lifetime
            Character.LifeTime = Character.LifeTime + 1;
            
            CheckForWin();
        }
        
        public bool GrabTheClickedProduct(Product product)
        {
            // Returns a bool depending on if the product should be removed from inventory or not
            string stringname;
            Product clickedProduct = product;
            if (clickedProduct.WantToSell)
            {
                if (Character.Currency >= clickedProduct.SellsFor)
                {

                    Character.Currency = Character.Currency - clickedProduct.SellsFor;
                    var test1 = Character.Inventory.Count;

                    Character.Inventory.Add(product);

                    var test2 = Character.Inventory.Count;
                    return true;

                }
                else
                {
                    MessageBox.Show("Not enough currency!"); 
                    return false;
                }
            }
            else
            {
                // Checks if the item the character wants to sell is in the inventory
                Character.Currency = Character.Currency + clickedProduct.BuyersPrice;
                stringname = product.ProductType;

                Product doWeHaveIt = Character.Inventory.Where(i => i.ProductType == stringname).FirstOrDefault();

                if (doWeHaveIt != null)
                {
                    Character.Inventory.Remove(Character.Inventory.First(s => s.ProductType == stringname));
                    return true;
                }
                else
                {
                    MessageBox.Show("You don't have this item to sell!");
                    return false;
                }
            }
        }

        private void CheckForWin()
        {
            if (Character.LifeTime == 30)
            {
                MessageBox.Show("Time's up! You died!");
                // nulstil alt
            }

            if (Character.Currency >= 5000)
            {
                MessageBox.Show("Congrats, you reached 5000g! You won! :)");
                // nulstil alt
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var img = new BitmapImage();
            System.Uri uri = new System.Uri("C:\\Users\\reeth\\Documents\\Welp\\product (1).png");
            System.Uri uri2 = new System.Uri("C:\\Users\\reeth\\Documents\\Welp\\product (3).png");
            System.Uri uri3 = new System.Uri("C:\\Users\\reeth\\Documents\\Welp\\product (5).png");
            System.Uri uri4 = new System.Uri("C:\\Users\\reeth\\Documents\\Welp\\product (6).png");
            System.Uri uri5 = new System.Uri("C:\\Users\\reeth\\Documents\\Welp\\product (7).png");

            switch (value.ToString().ToLower())
            {
                case "cabbage":
                    return new BitmapImage(uri);
                case "strawberry":
                    return new BitmapImage(uri2);
                case "fish":
                    return new BitmapImage(uri3);
                case "egg":
                    return new BitmapImage(uri4);
                case "wheat":
                    return new BitmapImage(uri5);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
