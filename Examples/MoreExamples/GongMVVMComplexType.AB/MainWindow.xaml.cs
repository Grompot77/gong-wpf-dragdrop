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
using GongComplexType.ViewModels;

namespace GongComplexType
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new SomeViewModel();
        }

        public SomeViewModel ViewModel
        {
            get { return DataContext as SomeViewModel; }
            set { DataContext = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(string.Format("Selected: {0}\r\nAvailable: {1}", ViewModel.Selected.Count, ViewModel.MainCollection.Count));
        }
    }
}
