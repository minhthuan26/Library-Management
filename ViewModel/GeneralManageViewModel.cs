using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.ViewModel
{
    public class GeneralManageViewModel : BaseViewModel
    {
        private int _bookCount;
        public int BookCount { get { if (_bookCount == null) _bookCount = 0; return _bookCount; } set { _bookCount = value; OnPropertyChanged(); } }
        private int _issueBookDetailCount;
        public int IssueBookDetailCount { get { if (_issueBookDetailCount == null) _issueBookDetailCount = 0; return _issueBookDetailCount; } set { _issueBookDetailCount = value; OnPropertyChanged(); } }
        private int _indemnifyDetailCount;
        public int IndemnifyDetailCount { get { if (_indemnifyDetailCount == null) _indemnifyDetailCount = 0; return _indemnifyDetailCount; } set { _indemnifyDetailCount = value; OnPropertyChanged(); } }
        private int _customerCount;
        public int CustomerCount { get { if (_customerCount == null) _customerCount = 0; return _customerCount; } set { _customerCount = value; OnPropertyChanged(); } }

        public GeneralManageViewModel(int BookCount, int IssueBookDetailCount, int IndemnifyDetailCount, int CustomerCount)
        {
            this.BookCount = BookCount;
            this.IssueBookDetailCount = IssueBookDetailCount;
            this.IndemnifyDetailCount= IndemnifyDetailCount;
            this.CustomerCount = CustomerCount;
        }

        public GeneralManageViewModel()
        {
            this.BookCount = 0;
            this.IssueBookDetailCount = 0;
            this.IndemnifyDetailCount = 0;
            this.CustomerCount = 0;
            getGeneral();
        }
        public GeneralManageViewModel getGeneral()
        {

            var bookList = DataProvider.Ins.DB.Saches;
            if (bookList == null)
                BookCount = 0;
            else
                foreach(var item in bookList)
                {
                    BookCount += item.SoLuong; 
                }

            var IssueDetailList = DataProvider.Ins.DB.ChiTietPhieuMuons;
            if (IssueDetailList == null)
                IssueBookDetailCount = 0;
            else
                foreach (var item in IssueDetailList)
                {
                    IssueBookDetailCount += item.SoLuong;
                }

            var indemnifyList = DataProvider.Ins.DB.ChiTietPhieuBoiThuongs;
            if (indemnifyList == null)
                IndemnifyDetailCount = 0;
            
            else
                foreach (var item in indemnifyList)
                {
                    IndemnifyDetailCount += item.SoLuong;
                }

            var customerList = DataProvider.Ins.DB.KhachHangs;
            if (customerList == null)
                CustomerCount = 0;
            else
                foreach (var item in customerList)
                {
                    CustomerCount ++;
                }

            return new GeneralManageViewModel(BookCount, IssueBookDetailCount, IndemnifyDetailCount, CustomerCount);
        }
    }
}
