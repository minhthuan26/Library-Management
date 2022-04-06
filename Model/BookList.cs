using QuanLyThuVien.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Model
{
    public class BookList : BaseViewModel
    {
        private int _stt;
        public int STT { get { return _stt; } set { _stt = value; OnPropertyChanged(); } }
        private Sach _book;
        public Sach Book { get { return _book; } set { _book = value; OnPropertyChanged(); } }
        private string _hoTenTacGia;
        public string HoTenTacGia { get { return _hoTenTacGia; } set { _hoTenTacGia = value; OnPropertyChanged(); } }
    }
}
