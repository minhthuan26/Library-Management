using QuanLyThuVien.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Model
{
    public class RoleList : BaseViewModel
    {
        private int _stt;
        public int STT { get { return _stt; } set { _stt = value; OnPropertyChanged(); } }
        private ChucVu _role;
        public ChucVu Role { get { return _role; } set { _role = value; OnPropertyChanged(); } }
    }
}
