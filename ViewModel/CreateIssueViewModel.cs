using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyThuVien.ViewModel
{
    public class CreateIssueViewModel : BaseViewModel
    {
        private ObservableCollection<KhachHang> _customerList;
        public ObservableCollection<KhachHang> CustomerList { get { return _customerList; } set { _customerList = value; OnPropertyChanged(); } }
        private ObservableCollection<Sach> _bookList;
        public ObservableCollection<Sach> BookList { get { return _bookList; } set { _bookList = value; OnPropertyChanged(); } }

        //private string _bookQuantity;
        //public string BookQuantity { get { return _bookQuantity; } set { _bookQuantity = value; OnPropertyChanged(); } }
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

        //private string _detailID;
        //public string DetailID
        //{
        //    get
        //    {
        //        if (_detailID == null)
        //            _detailID = Guid.NewGuid().ToString();
        //        return _detailID;
        //    }
        //    set
        //    {
        //        _detailID = value;
        //        OnPropertyChanged();
        //    }
        //}

        private int _rest1 = -1;
        public int Rest1
        {
            get
            {
                return _rest1;
            }
            set
            {
                _rest1 = value;
                OnPropertyChanged();
            }
        }

        private int _rest2 = -1;
        public int Rest2
        {
            get
            {
                return _rest2;
            }
            set
            {
                _rest2 = value;
                OnPropertyChanged();
            }
        }

        private int _rest3 = -1;
        public int Rest3
        {
            get
            {
                return _rest3;
            }
            set
            {
                _rest3 = value;
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

        private bool _isActive1;
        public bool IsActive1 { get { return _isActive1; } set { _isActive1 = value; OnPropertyChanged(); } }
        private bool _isActive2;
        public bool IsActive2 { get { return _isActive2; } set { _isActive2 = value; OnPropertyChanged(); } }
        
        private Sach _selectedBook1;
        public Sach SelectedBook1
        {
            get
            {
                return (Sach)_selectedBook1;
            }
            set
            {
                _selectedBook1 = value;
                if (SelectedBook1 != null)
                {
                    Sach book = DataProvider.Ins.DB.Saches.Where(x => x.ID == SelectedBook1.ID && x.TrangThai == 1).SingleOrDefault();
                    int restQuantity = book.SoLuong;
                    Rest1 = restQuantity;
                    IsActive1 = true;
                }
                else
                {
                    IsActive1 = false;
                    Rest1 = -1;
                }
                OnPropertyChanged();
            }
        }

        private Sach _selectedBook2;
        public Sach SelectedBook2
        {
            get
            {
                return (Sach)_selectedBook2;
            }
            set
            {
                _selectedBook2 = value;
                if (_selectedBook2 != null)
                {
                    Sach book = DataProvider.Ins.DB.Saches.Where(x => x.ID == SelectedBook2.ID && x.TrangThai == 1).SingleOrDefault();
                    int restQuantity = book.SoLuong;
                    Rest2 = restQuantity;
                    IsActive2 = true;
                }
                else
                {
                    IsActive2 = false;
                    Rest2 = -1;
                }
                OnPropertyChanged();
            }
        }

        private Sach _selectedBook3;
        public Sach SelectedBook3
        {
            get
            {
                return (Sach)_selectedBook3;
            }
            set
            {
                _selectedBook3 = value;
                if (_selectedBook3 != null)
                {
                    Sach book = DataProvider.Ins.DB.Saches.Where(x => x.ID == SelectedBook3.ID && x.TrangThai == 1).SingleOrDefault();
                    int restQuantity = book.SoLuong;
                    Rest3 = restQuantity;
                }
                else
                {
                    Rest3 = -1;
                }
                OnPropertyChanged();
            }
        }

        public ICommand CancelCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand1 { get; set; }
        public ICommand DeleteCommand2 { get; set; }
        public ICommand DeleteCommand3 { get; set; }
        public CreateIssueViewModel()
        {
            setDefault();
            CancelCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                p.Close();
            });

            SaveCommand = new RelayCommand<Window>((p) =>
            {
                if (SelectedCustomer == null
                    || (SelectedBook1 == null && SelectedBook2 == null && SelectedBook3 == null))
                    return false;
                if (Rest1 == 0 || Rest2 == 0 || Rest3 == 0)
                    return false;
                return true;
            }, (p) =>
            {
                try
                {   
                    PhieuMuon phieuMuon = new PhieuMuon();
                    phieuMuon.ID = ID;
                    phieuMuon.IDNguoiLap = Staff.ID;
                    phieuMuon.NgayLapPhieu = Date;
                    phieuMuon.TrangThai = 1;
                    phieuMuon.KhachHang = SelectedCustomer;
                    DataProvider.Ins.DB.AddToPhieuMuons(phieuMuon);

                    if (SelectedBook1 != null)
                    {
                        ChiTietPhieuMuon chiTiet = new ChiTietPhieuMuon();
                        chiTiet.ID = Guid.NewGuid().ToString();
                        chiTiet.PhieuMuon = phieuMuon;
                        chiTiet.Sach = SelectedBook1;
                        chiTiet.SoLuong = 1;
                        DataProvider.Ins.DB.AddToChiTietPhieuMuons(chiTiet);
                        Sach sach = DataProvider.Ins.DB.Saches.Where(x => x.ID == SelectedBook1.ID).SingleOrDefault();
                        sach.SoLuong--;
                    }

                    if (SelectedBook2 != null)
                    {
                        ChiTietPhieuMuon chiTiet = new ChiTietPhieuMuon();
                        chiTiet.ID = Guid.NewGuid().ToString();
                        chiTiet.PhieuMuon = phieuMuon;
                        chiTiet.Sach = SelectedBook2;
                        chiTiet.SoLuong = 1;
                        DataProvider.Ins.DB.AddToChiTietPhieuMuons(chiTiet);
                        Sach sach = DataProvider.Ins.DB.Saches.Where(x => x.ID == SelectedBook2.ID).SingleOrDefault();
                        sach.SoLuong--;
                    }

                    if (SelectedBook3 != null)
                    {
                        ChiTietPhieuMuon chiTiet = new ChiTietPhieuMuon();
                        chiTiet.ID = Guid.NewGuid().ToString();
                        chiTiet.PhieuMuon = phieuMuon;
                        chiTiet.Sach = SelectedBook3;
                        chiTiet.SoLuong = 1;
                        DataProvider.Ins.DB.AddToChiTietPhieuMuons(chiTiet);
                        Sach sach = DataProvider.Ins.DB.Saches.Where(x => x.ID == SelectedBook3.ID).SingleOrDefault();
                        sach.SoLuong--;
                    }

                    DataProvider.Ins.DB.SaveChanges();
                    MessageBox.Show("Đã lập phiếu mượn.");
                    p.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra, không thể lập phiếu.");
                }
            });

            DeleteCommand1 = new RelayCommand<object>((p) =>
            {
                if (SelectedBook1 != null)
                    return true;
                return false;
            }, (p) =>
            {
                SelectedBook1 = null;
            });

            DeleteCommand2 = new RelayCommand<object>((p) =>
            {
                if (SelectedBook2 != null)
                    return true;
                return false;
            }, (p) =>
            {
                SelectedBook2 = null;
            });

            DeleteCommand3 = new RelayCommand<object>((p) =>
            {
                if (SelectedBook3 != null)
                    return true;
                return false;
            }, (p) =>
            {
                SelectedBook3 = null;
            });
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
