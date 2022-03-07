using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyThuVien.ViewModel
{
    public class BookManageViewModel : BaseViewModel
    {
        private ObservableCollection<BookList> _list;
        public ObservableCollection<BookList> List { get { return _list;} set { _list = value; OnPropertyChanged(); } }
        private string _ten;
        public string Ten { get { return _ten; } set { _ten = value; OnPropertyChanged();} }
        private int? _soLuong;
        public int? SoLuong { get { return _soLuong; } set { _soLuong = value; OnPropertyChanged(); } }
        private double? _gia;
        public double? Gia { get { return _gia; } set { _gia = value; OnPropertyChanged(); } }
        private DateTime? _ngayXuatBan;
        public DateTime? NgayXuatBan { get { return _ngayXuatBan; } set { _ngayXuatBan = value; OnPropertyChanged(); } }
        private BookList _selectedItem;
        public BookList SelectedItem 
        { 
            get 
            {
                IsEditClick = false;
                return (BookList) _selectedItem; 
            } 
            set 
            {
                _selectedItem = value; 
                if (SelectedItem != null)
                {
                    BaseViewModel.SelectedItem = SelectedItem;
                    Ten = SelectedItem.Book.TenSach;
                    SoLuong = SelectedItem.Book.SoLuong;
                    Gia = SelectedItem.Book.Gia;
                    NgayXuatBan = SelectedItem.Book.NgayXuatBan;
                }
                OnPropertyChanged();
            } 
        }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public BookManageViewModel()
        {
            loadList();
            SaveCommand = new RelayCommand<Button>((p) =>
            {
                if (IsEditClick)
                {
                    IsEnable = true;
                    return true;
                } 
                return false;
            }, (p) =>
            {
                
            });
            CancelCommand = new RelayCommand<Button>((p) => 
            {
                if (IsEditClick)
                {
                    IsEnable = true;
                    return true;
                }
                return false;
            }, (p) =>
            {
                IsEnable = false;
                IsEditClick = false; 
            });
        }
        public void loadList()
        {
            List = new ObservableCollection<BookList>();
                var bookList = DataProvider.Ins.DB.Saches;
                int index = 1;
                foreach (var book in bookList)
                {
                    BookList item = new BookList();
                    item.STT = index;
                    item.Book = book;
                    List.Add(item);
                    index++;
                }
            }
    }
}
