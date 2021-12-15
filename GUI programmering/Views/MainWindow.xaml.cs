using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Windows.Media.Animation;
using GUI_programmering.ViewModels;
using System.Security.Policy;

namespace GUI_programmering.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public MainWindowViewModel MainViewVm = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += LoadedWindow;
        }

        private void LoadedWindow(object sender, RoutedEventArgs args)
        {
            DataContext = MainViewVm;
        }

        private void Building1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MoveCharacter(170, 250);
            MainViewVm.GrabTheClickedBuilding("Arena");
            GetBoard("Arena");

        }

        private void Building2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MoveCharacter(160, 620);
            MainViewVm.GrabTheClickedBuilding("Stronghold");
            GetBoard("Stronghold");
        }

        private void Building3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MoveCharacter(500, 300);
            MainViewVm.GrabTheClickedBuilding("Cathedral");
            GetBoard("Cathedral");
        }

        private void Building4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MoveCharacter(500, 580);
            MainViewVm.GrabTheClickedBuilding("Castle");
            GetBoard("Castle");
        }

        private void MoveCharacter(int newPositionTop, int newPositionLeft)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 1;
            animation.To = 0;
            animation.Duration = TimeSpan.FromSeconds(1);

            animation.Completed += (s, a) =>
            {
                Vector offset = VisualTreeHelper.GetOffset(Character);
                var top = offset.Y;
                var left = offset.X;
                TranslateTransform trans = new TranslateTransform();
                Character.RenderTransform = trans;
                DoubleAnimation anim1 = new DoubleAnimation(top, newPositionTop - top, TimeSpan.FromSeconds(0));
                DoubleAnimation anim2 = new DoubleAnimation(left, newPositionLeft - left, TimeSpan.FromSeconds(0));
                trans.BeginAnimation(TranslateTransform.YProperty, anim1);
                trans.BeginAnimation(TranslateTransform.XProperty, anim2);
            };
            Character.BeginAnimation(OpacityProperty, animation); // eller widthproperty for zoom

            animation.Completed += (o, e) =>
            {
                DoubleAnimation animation2 = new DoubleAnimation();
                animation2.From = 0;
                animation2.To = 1;
                animation2.Duration = TimeSpan.FromSeconds(1);
                Character.BeginAnimation(OpacityProperty, animation2);
            };
        }

        private void GetBoard(string building)
        {
            CityName.Content = "Visiting " + building;
            Board.Visibility = Visibility.Visible;
            GoBack.Visibility = Visibility.Visible;
            CityName.Visibility = Visibility.Visible;
            WeSell.Visibility = Visibility.Visible;
            WeBuy.Visibility = Visibility.Visible;
            ItemsForSale.Visibility = Visibility.Visible;
            BuysFor.Visibility = Visibility.Visible;
        }

        private void GoBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RemoveBoard();
        }

        private void RemoveBoard()
        {
            Board.Visibility = Visibility.Hidden;
            GoBack.Visibility = Visibility.Hidden;
            CityName.Visibility = Visibility.Hidden;
            WeSell.Visibility = Visibility.Hidden;
            WeBuy.Visibility = Visibility.Hidden;
            ItemsForSale.Visibility = Visibility.Hidden;
            BuysFor.Visibility = Visibility.Hidden;
        }

        private void ProductTypeForSale_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Teknisk set kunne vi godt oprette en eventhandler, så VMen subscriber til om der bliver klikket her.
            // Men det kræver en instantiering af MainWindow, og så laver vi et alvorligt brud på MVVM-mønstret.
            // I stedet hapser vi det medsendte objekt, hiver datacontexten ud ad denne og sender det afsted til en
            // metode i VM. Viewet må altid gerne tale til VM - ikke omvendt.
            
            var productClicked = (sender as TextBlock).DataContext as Product;
            bool shouldNotBeShown = MainViewVm.GrabTheClickedProduct(productClicked);
            if (shouldNotBeShown)
            {
                (sender as TextBlock).Visibility = Visibility.Hidden;
            }

        }

        
    }
}
