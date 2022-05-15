using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.ViewModel
{
    public class CreateIssueViewModel : BaseViewModel
    {
        private ObservableCollection<KhachHang> _customerList;
        public ObservableCollection<KhachHang> CustomerList { get { return _customerList; } set { _customerList = value; OnPropertyChanged(); } }
        private ObservableCollection<Sach> _bookList;
        public ObservableCollection<Sach> BookList { get { return _bookList; } set { _bookList = value; OnPropertyChanged(); } }
        private string _id;
        public string ID 
        { 
            get 
            { 
                if(_id == null)
                    _id = Guid.NewGuid().ToString();
                return _id; 
            } 
            set 
            { 
                _id = value;
                OnPropertyChanged();
            } 
        }

        private string _detailID;
        public string DetailID
        {
            get
            {
                if (_detailID == null)
                    _detailID = Guid.NewGuid().ToString();
                return _detailID;
            }
            set
            {
                _detailID = value;
                OnPropertyChanged();
            }
        }

        private string _rest;
        public string Rest
        {
            get
            {
                if (_rest == null)
                    _rest = "(Còn lại: 0)";
                return _rest;
            }
            set
            {
                _rest = value;
                OnPropertyChanged();
            }
        }

        private NhanVien _staff;
        public NhanVien Staff
        {
            get
            {
                if (_staff == null)
                    _staff = LoginViewModel.CurrentAccount.NhanVien;
                return _staff;
            }
            set
            {
                _staff = value;
                OnPropertyChanged();
            }
        }

        private DateTime _date;
        public DateTime Date
        {
            get
            {
                _date = DateTime.Now;
                return _date;
            }
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        private KhachHang _selectedCustomer;
        public KhachHang SelectedCustomer
        {
            get
            {
                return (KhachHang)_selectedCustomer;
            }
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();
            }
        }

        private Sach _selectedBook;
        public Sach SelectedBook
        {
            get
            {
                return (Sach)_selectedBook;
            }
            set
            {
                _selectedBook = value;
                if (SelectedBook != null)
                {
                    Sach book = DataProvider.Ins.DB.Saches.Where(x => x.ID == SelectedBook.ID && x.TrangThai == 1).SingleOrDefault();
                    int restQuantity = book.SoLuong;
                    Rest = "(Còn lại: " + restQuantity + ")";
                }
                else
                {
                    Rest = null;
                }
                OnPropertyChanged();
            }
        }

        public CreateIssueViewModel()
        {
            setDefault();
        }

        public void setDefault()
        {
            CustomerList = new ObservableCollection<KhachHang>(DataProvider.Ins.DB.KhachHangs);
            BookList = new ObservableCollection<Sach>(DataProvider.Ins.DB.Saches);
            BaseViewModel.SelectedItem = null;
            IsEnable = false;
            IsClick = false;
            IsAdd = false;
            IsDelete = false;
        }
    }
}
