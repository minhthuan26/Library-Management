using QuanLyThuVien.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Model
{
    public class CustomerList : BaseViewModel
    {
        private int _stt;
        public int STT { get { return _stt; } set { _stt = value; OnPropertyChanged(); } }
        private KhachHang _customer;
        public KhachHang Customer { get { return _customer; } set { _customer = value; OnPropertyChanged(); } }
    }
}
