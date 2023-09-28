using Reolmarkedet.ModelViews;
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

namespace Reolmarkedet.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {

        
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void OpretLejer_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new TenantViewModel();
        }

        private void StartSalg_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new SaleViewModels();
        }

        //private void Create_Sale_Button_Click(object sender, RoutedEventArgs e)
        //{
        //    Salg s = new();
        //    s.Show();

        //}



        //private void Create_Tenant_Button_Click(object sender, RoutedEventArgs e)
        //{
        //OpretLejer Ol = new();
        //Ol.Show();
        //}

        //private void Update_Tenant_Button_Click(object sender, RoutedEventArgs e)
        //{
        //    OpretLejer Ol = new();
        //    Ol.Show();
        //}

        //private void Delete_Tenant_Button_Click(object sender, RoutedEventArgs e)
        //{
        //    OpretLejer Ol = new();
        //    Ol.Show();
        //}
    }
}
