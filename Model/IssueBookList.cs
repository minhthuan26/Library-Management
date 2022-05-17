using QuanLyThuVien.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Model
{
    public class IssueBookList : BaseViewModel
    {
        private int _stt;
        public int STT { get { return _stt; } set { _stt = value; OnPropertyChanged(); } }

        private PhieuMuon _issueBook;
        public PhieuMuon IssueBook { get { return _issueBook; } set { _issueBook = value; OnPropertyChanged(); } }

        private ChiTietPhieuMuon _detailIssueBook;
        public ChiTietPhieuMuon DetailIssueBook { get { return _detailIssueBook; } set { _detailIssueBook = value; OnPropertyChanged(); } }
    }
}
