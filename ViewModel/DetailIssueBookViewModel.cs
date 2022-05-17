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
    public class DetailIssueBookViewModel : BaseViewModel
    {
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

        public DetailIssueBookViewModel()
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
            SelectedItem = (IssueBookList)BaseViewModel.SelectedItem;
            var chiTiet = ((IEnumerable)DataProvider.Ins.DB.ChiTietPhieuMuons.Where(x => x.IDPhieuMuon == SelectedItem.IssueBook.ID)).Cast<object>().ToArray();
            ChiTietPhieuMuon detail = (ChiTietPhieuMuon)chiTiet[0];
            Book1 = detail.Sach;
            Customer = SelectedItem.IssueBook.KhachHang;
            Staff = SelectedItem.IssueBook.NhanVien;
            if (SelectedItem != null)
            {
                if (SelectedItem.IssueBook.TrangThai == 1)
                    _status = "Chưa trả sách";
                else
                    _status = "Đã trả sách";
            }
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
