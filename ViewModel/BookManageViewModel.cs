using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyThuVien.ViewModel
{
    public class BookManageViewModel : BaseViewModel
    {
        private ObservableCollection<BookList> _list;
        public ObservableCollection<BookList> List { get { return _list;} set { _list = value; OnPropertyChanged(); } }
        private ObservableCollection<TacGia> _authorList;
        public ObservableCollection<TacGia> AuthorList { get { return _authorList; } set { _authorList = value; OnPropertyChanged(); } }
        private ObservableCollection<TheLoai> _bookTypeList;
        public ObservableCollection<TheLoai> BookTypeList { get { return _bookTypeList; } set { _bookTypeList = value; OnPropertyChanged(); } }
        private string _ten;
        public string Ten { get { return _ten; } set { _ten = value; OnPropertyChanged();} }
        private int _soLuong;
        public int SoLuong { get { return _soLuong; } set { _soLuong = value; OnPropertyChanged(); } }
        private double _gia;
        public double Gia { get { return _gia; } set { _gia = value; OnPropertyChanged(); } }
        private DateTime _ngayXuatBan;
        public DateTime NgayXuatBan { get { return _ngayXuatBan; } set { _ngayXuatBan = value; OnPropertyChanged(); } }
        private BookList _selectedItem;
        public BookList SelectedItem 
        { 
            get 
            {
                IsClick = false;
                return (BookList) _selectedItem; 
            } 
            set 
            {
                _selectedItem = value; 
                if (SelectedItem != null)
                {
                    BaseViewModel.SelectedItem = SelectedItem;
                    IsEnable = false;
                    Ten = SelectedItem.Book.TenSach;
                    SoLuong = SelectedItem.Book.SoLuong;
                    Gia = SelectedItem.Book.Gia;
                    NgayXuatBan = SelectedItem.Book.NgayXuatBan;
                    SelectedAuthor = SelectedItem.Book.TacGia;
                    SelectedBookType = SelectedItem.Book.TheLoai;
                }
                if (SelectedItem == null)
                {
                    Ten = default;
                    SoLuong = default;
                    Gia = default;
                    NgayXuatBan = DateTime.Now;
                    SelectedAuthor = default;
                    SelectedBookType = default;
                }
                OnPropertyChanged();
            } 
        }
        private TacGia _selectedAuthor;
        public TacGia SelectedAuthor
        {
            get
            {
                IsClick = false;
                return (TacGia)_selectedAuthor;
            }
            set
            {
                _selectedAuthor = value;
                OnPropertyChanged();
            }
        }

        private TheLoai _selectedBookType;
        public TheLoai SelectedBookType
        {
            get
            {
                IsClick = false;
                return (TheLoai)_selectedBookType;
            }
            set
            {
                _selectedBookType = value;
                OnPropertyChanged();
            }
        }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public BookManageViewModel()
        {
            setDefault();
            SaveCommand = new RelayCommand<Button>((p) =>
            {
                if (IsClick || IsEnable)
                {
                    if (string.IsNullOrEmpty(Ten) || NgayXuatBan < DateTime.Parse("01/01/1900") || NgayXuatBan == null || SoLuong < 0)
                    {
                        return false;
                    }
                    IsEnable = true;
                    return true;
                }
                
                return false;
            }, (p) =>
            {
                if(BaseViewModel.SelectedItem != null && !IsAdd)
                {
                    var book = DataProvider.Ins.DB.Saches.Where(x => x.ID == SelectedItem.Book.ID).SingleOrDefault();
                    book.TenSach = Ten;
                    book.SoLuong = SoLuong;
                    book.Gia = Gia;
                    book.IDTacGia = SelectedAuthor.ID;
                    book.IDTheLoai = SelectedBookType.ID;
                    book.NgayXuatBan = NgayXuatBan;
                    DataProvider.Ins.DB.SaveChanges();
                    setDefault();
                }
                else if(IsAdd)
                {
                    var book = new Sach();
                    book.ID = Guid.NewGuid().ToString();
                    book.TenSach = Ten;
                    book.SoLuong = SoLuong;
                    book.Gia = Gia;
                    book.IDTacGia = SelectedAuthor.ID;
                    book.IDTheLoai = SelectedBookType.ID;
                    book.NgayXuatBan = NgayXuatBan;
                    book.TrangThai = "1";
                    DataProvider.Ins.DB.AddToSaches(book);
                    DataProvider.Ins.DB.SaveChanges();
                    setDefault();
                }
                else
                {
                    setDefault();
                }
                
                //MessageBox.Show(book.TenSach.ToString());
            });
            CancelCommand = new RelayCommand<Button>((p) => 
            {
                if (IsClick || IsEnable)
                {
                    IsEnable = true;
                    return true;
                }
                return false;
            }, (p) =>
            {
                setDefault();
                
            });

            //DeleteCommand = new RelayCommand<Button>((p) =>
            //{
            //    return true;
            //}, (p) =>
            //{
            //    DataProvider.Ins.DB.DeleteObject(SelectedItem);
            //    DataProvider.Ins.DB.SaveChanges();
            //    setDefault();
            //});
            
        }

        public void setDefault()
        {
            AuthorList = new ObservableCollection<TacGia>(DataProvider.Ins.DB.TacGias);
            BookTypeList = new ObservableCollection<TheLoai>(DataProvider.Ins.DB.TheLoais);
            BaseViewModel.SelectedItem = null;
            this.SelectedItem = null;
            IsEnable = false;
            IsClick = false;
            IsAdd = false;
            if(IsDelete)
                List.Remove(SelectedItem);
            IsDelete = false;
            
            loadList();
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
                item.HoTenTacGia = book.TacGia.Ho + " " + book.TacGia.Ten;
                List.Add(item);
                index++;
            }
        }
    }
}
