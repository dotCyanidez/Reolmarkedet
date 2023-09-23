﻿using Reolmarkedet.ModelViews;
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
using System.Windows.Shapes;

namespace Reolmarkedet.Views
{
    /// <summary>
    /// Interaction logic for Salg.xaml
    /// </summary>
    public partial class Salg : Window
    {

        SaleViewModels svm = new();
        public Salg()
        {
            InitializeComponent();
            DataContext = svm;
        }
    }
}
