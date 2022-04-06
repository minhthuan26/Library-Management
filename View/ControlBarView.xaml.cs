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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyThuVien.UserControlMain
{
    /// <summary>
    /// Interaction logic for UserControlUC.xaml
    /// </summary>
    public partial class ControlBarView : UserControl
    {
        public ControlBarViewModel Viewmodel { get; set; }
        public ControlBarView()
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new ControlBarViewModel();
        }
    }
}
