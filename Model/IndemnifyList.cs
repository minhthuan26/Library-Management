using QuanLyThuVien.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Model
{
    public class IndemnifyList : BaseViewModel
    {
        private int _stt;
        public int STT { get { return _stt; } set { _stt = value; OnPropertyChanged(); } }

        private PhieuBoiThuong _idemnifyBook;
        public PhieuBoiThuong IndemnifyBook { get { return _idemnifyBook; } set { _idemnifyBook = value; OnPropertyChanged(); } }

        private ChiTietPhieuBoiThuong _detailidemnifyBook;
        public ChiTietPhieuBoiThuong DetailIndemnifyBook { get { return _detailidemnifyBook; } set { _detailidemnifyBook = value; OnPropertyChanged(); } }
    }
}
