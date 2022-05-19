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
    public class DetailIndemnifyBookViewModel : BaseViewModel
    {
        private IndemnifyList _selectedItem;
        public IndemnifyList SelectedItem
        {
            get
            {
                return (IndemnifyList)_selectedItem;
            }
            set
            {
                _selectedItem = value;
                _selectedItem = (IndemnifyList)BaseViewModel.SelectedItem;
                OnPropertyChanged();
            }
        }

        private KhachHang _customer;
        public KhachHang Customer { get { return _customer; } set { _customer = value; OnPropertyChanged(); } }

        private NhanVien _staff;
        public NhanVien Staff { get { return _staff; } set { _staff = value; OnPropertyChanged(); } }
        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

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

        public ICommand ConfirmCommand { get; set; }

        public DetailIndemnifyBookViewModel()
        {
            setValues();
            ConfirmCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                p.Close();
            });
        }

        private void setValues()
        {
            SelectedItem = (IndemnifyList)BaseViewModel.SelectedItem;
            var chiTiet = ((IEnumerable)DataProvider.Ins.DB.ChiTietPhieuBoiThuongs.Where(x => x.IDPhieuBoiThuong == SelectedItem.IndemnifyBook.ID)).Cast<object>().ToArray();
            if(chiTiet.Length == 1 )
            {
                ChiTietPhieuBoiThuong detail = (ChiTietPhieuBoiThuong)chiTiet[0];
                if(detail.Gia != 0)
                    Book1 = detail.Sach;
            }
            Customer = SelectedItem.IndemnifyBook.PhieuMuon.KhachHang;
            Staff = SelectedItem.IndemnifyBook.NhanVien;
            if (SelectedItem != null)
            {
                if (SelectedItem.IndemnifyBook.TrangThai == 1)
                    _status = "Chưa thanh toán";
                else
                    _status = "Đã thanh toán";
            }
            if (chiTiet.Length > 1)
            {
                ChiTietPhieuBoiThuong detail = (ChiTietPhieuBoiThuong)chiTiet[1];
                if (detail.Gia != 0)
                    Book2 = detail.Sach;
            }
            if (chiTiet.Length == 3)
            {
                ChiTietPhieuBoiThuong detail = (ChiTietPhieuBoiThuong)chiTiet[2];
                if (detail.Gia != 0)
                    Book3 = detail.Sach;
            }
        }
    }
}
