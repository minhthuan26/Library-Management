using QuanLyThuVien.ViewModel;
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

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for CompensationWindow.xaml
    /// </summary>
    public partial class CompensationWindow : Window
    {
        public CompensationWindow()
        {
            InitializeComponent();
            DataContext = new CompensationViewModel();
        }
    }
}
