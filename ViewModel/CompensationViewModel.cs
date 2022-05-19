using QuanLyThuVien.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyThuVien.ViewModel
{
    public class CompensationViewModel : BaseViewModel
    {
        private string _id;
        public string ID
        {
            get
            {
                if (_id == null)
                    _id = Guid.NewGuid().ToString();
                return _id;
            }
            set
            {
                _id = value;
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

        private IssueBookList _selectedItem;
        public IssueBookList SelectedItem
        {
            get
            {
                return (IssueBookList)_selectedItem;
            }
            set
            {
                _selectedItem = value;
                _selectedItem = (IssueBookList)BaseViewModel.SelectedItem;
                OnPropertyChanged();
            }
        }

        private KhachHang _customer;
        public KhachHang Customer { get { return _customer; } set { _customer = value; OnPropertyChanged(); } }

        private Sach _book1;
        public Sach Book1
        {
            get { return _book1; }
            set
            {
                _book1 = value;
                OnPropertyChanged();
            }
        }

        private Sach _book2;
        public Sach Book2
        {
            get { return _book2; }
            set
            {
                _book2 = value;
                OnPropertyChanged();
            }
        }

        private Sach _book3;
        public Sach Book3
        {
            get { return _book3; }
            set
            {
                _book3 = value;
                OnPropertyChanged();
            }
        }

        private float _price1;
        public float Price1
        {
            get
            {
                return _price1;
            }
            set
            {
                _price1 = value;
                Total += _price1;
                OnPropertyChanged();
            }
        }

        private float _price2;
        public float Price2
        {
            get
            {
                return _price2;
            }
            set
            {
                _price2 = value;
                Total += _price2;
                OnPropertyChanged();
            }
        }

        private float _price3;
        public float Price3
        {
            get
            {
                return _price3;
            }
            set
            {
                _price3 = value;
                Total += _price3;
                OnPropertyChanged();
            }
        }

        private float _total;
        public float Total
        {
            get
            {
                return _total;
            }
            set 
            {
                _total = value;
                _total = Price1 + Price2 + Price3;
                OnPropertyChanged();
            }
        }

        public ICommand CancelCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public CompensationViewModel()
        {
            setValues();
            CancelCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                p.Close();
            });

            SaveCommand = new RelayCommand<Window>((p) =>
            {
                if (Price1 == 0 && Price2 == 0 && Price3 == 0)
                    return false;
                return true;
            }, (p) =>
            {
                try
                {
                    PhieuBoiThuong phieuBoiThuong = new PhieuBoiThuong();
                    phieuBoiThuong.ID = ID;
                    phieuBoiThuong.IDNguoiLap = Staff.ID;
                    phieuBoiThuong.NgayLapPhieu = Date;
                    phieuBoiThuong.TrangThai = 1;
                    phieuBoiThuong.PhieuMuon = SelectedItem.IssueBook;
                    phieuBoiThuong.PhieuMuon.KhachHang = Customer;
                    DataProvider.Ins.DB.AddToPhieuBoiThuongs(phieuBoiThuong);

                    if (Book1 != null)
                    {
                        ChiTietPhieuBoiThuong chiTiet = new ChiTietPhieuBoiThuong();
                        chiTiet.ID = Guid.NewGuid().ToString();
                        chiTiet.PhieuBoiThuong = phieuBoiThuong;
                        chiTiet.Sach = Book1;
                        chiTiet.SoLuong = 1;
                        chiTiet.Gia = Price1;
                        DataProvider.Ins.DB.AddToChiTietPhieuBoiThuongs(chiTiet);
                        if(Price1 == 0)
                        {
                            Sach sach = DataProvider.Ins.DB.Saches.Where(x => x.ID == Book1.ID).SingleOrDefault();
                            sach.SoLuong++;
                        }
                    }

                    if (Book2 != null)
                    {
                        ChiTietPhieuBoiThuong chiTiet = new ChiTietPhieuBoiThuong();
                        chiTiet.ID = Guid.NewGuid().ToString();
                        chiTiet.PhieuBoiThuong = phieuBoiThuong;
                        chiTiet.Sach = Book2;
                        chiTiet.SoLuong = 1;
                        chiTiet.Gia = Price2;
                        DataProvider.Ins.DB.AddToChiTietPhieuBoiThuongs(chiTiet);
                        if (Price2 == 0)
                        {
                            Sach sach = DataProvider.Ins.DB.Saches.Where(x => x.ID == Book2.ID).SingleOrDefault();
                            sach.SoLuong++;
                        }
                    }

                    if (Book3 != null)
                    {
                        ChiTietPhieuBoiThuong chiTiet = new ChiTietPhieuBoiThuong();
                        chiTiet.ID = Guid.NewGuid().ToString();
                        chiTiet.PhieuBoiThuong = phieuBoiThuong;
                        chiTiet.Sach = Book3;
                        chiTiet.SoLuong = 1;
                        chiTiet.Gia = Price3;
                        DataProvider.Ins.DB.AddToChiTietPhieuBoiThuongs(chiTiet);
                        if (Price3 == 0)
                        {
                            Sach sach = DataProvider.Ins.DB.Saches.Where(x => x.ID == Book3.ID).SingleOrDefault();
                            sach.SoLuong++;
                        }
                    }
                    PhieuMuon phieuMuon = SelectedItem.IssueBook;
                    phieuMuon.TrangThai = 0;
                    DataProvider.Ins.DB.SaveChanges();
                    MessageBox.Show("Đã lập phiếu bồi thường.");
                    p.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra, không thể lập phiếu.");
                }
            });
        }
        
        private void setValues()
        {
            SelectedItem = (IssueBookList)BaseViewModel.SelectedItem;
            var chiTiet = ((IEnumerable)DataProvider.Ins.DB.ChiTietPhieuMuons.Where(x => x.IDPhieuMuon == SelectedItem.IssueBook.ID)).Cast<object>().ToArray();
            ChiTietPhieuMuon detail = (ChiTietPhieuMuon)chiTiet[0];
            Book1 = detail.Sach;
            Customer = SelectedItem.IssueBook.KhachHang;
            Staff = SelectedItem.IssueBook.NhanVien;
            if (chiTiet.Length > 1)
            {
                ChiTietPhieuMuon detail2 = (ChiTietPhieuMuon)chiTiet[1];
                Book2 = detail2.Sach;
            }
            if (chiTiet.Length == 3)
            {
                ChiTietPhieuMuon detail3 = (ChiTietPhieuMuon)chiTiet[2];
                Book3 = detail3.Sach;
            }
        }
    }
}
