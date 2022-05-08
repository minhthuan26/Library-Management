using QuanLyThuVien.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Model
{
    public class BookTypeList : BaseViewModel
    {
        private int _stt;
        public int STT { get { return _stt; } set { _stt = value; OnPropertyChanged(); } }
        private TheLoai _bookType;
        public TheLoai BookType { get { return _bookType; } set { _bookType = value; OnPropertyChanged(); } }
    }
}
