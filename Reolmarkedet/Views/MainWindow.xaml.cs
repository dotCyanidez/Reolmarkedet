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

        BaseViewModels bvm = new();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = bvm;
        }

        private void Sale_Button_Click(object sender, RoutedEventArgs e)
        {
            Salg s = new();
            s.Show();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Lejer l = new();
            l.Show();
        }
    }
}
