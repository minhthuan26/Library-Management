using QuanLyThuVien.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Model
{
    public class StaffList : BaseViewModel
    {
        private int _stt;
        public int STT { get { return _stt; } set { _stt = value; OnPropertyChanged(); } }
        private NhanVien _staff;
        public NhanVien Staff { get { return _staff; } set { _staff = value; OnPropertyChanged(); } }
    }
}
