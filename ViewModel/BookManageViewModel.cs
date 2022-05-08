using QuanLyThuVien.Model;
using System;
using System.Collections;
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
        public ObservableCollection<BookList> List { get { return _list; } set { _list = value; OnPropertyChanged(); } }
        private ObservableCollection<TacGia> _authorList;
        public ObservableCollection<TacGia> AuthorList { get { return _authorList; } set { _authorList = value; OnPropertyChanged(); } }
        private ObservableCollection<TheLoai> _bookTypeList;
        public ObservableCollection<TheLoai> BookTypeList { get { return _bookTypeList; } set { _bookTypeList = value; OnPropertyChanged(); } }
        private string _ten;
        public string Ten { get { return _ten; } set { _ten = value; OnPropertyChanged(); } }
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
                return (BookList)_selectedItem;
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

        private List<string> _bookStatusSearchList = new List<string>()
        {
            "Tất cả",
            "Được lưu hành",
            "Đang cho mượn",
            "Hư hỏng / Mất"
        };
        public List<string> BookStatusSearchList
        {
            get
            {
                return _bookStatusSearchList;
            }
        }

        private List<string> _searchTypeList = new List<string>()
        {
            "Tất cả",
            "Theo tên",
            "Theo tác giả",
            "Theo thể loại",
            "Theo năm",
            "Theo giá"
        };
        public List<string> SearchTypeList
        {
            get
            {
                return _searchTypeList;
            }
        }
        private string _bookStatusSearch;
        public string BookStatusSearch
        {
            get
            {
                if (_bookStatusSearch == null)
                    _bookStatusSearch = BookStatusSearchList[0];
                return _bookStatusSearch;
            }
            set
            {
                _bookStatusSearch = value;
                OnPropertyChanged();
            }
        }

        private string _searchType;
        public string SearchType
        {
            get
            {
                if (_searchType == null)
                    _searchType = SearchTypeList[0];
                return _searchType;
            }
            set
            {
                _searchType = value;
                OnPropertyChanged();
            }
        }

        private string _searchValue;
        public string SearchValue
        {
            get
            {
                return _searchValue;
            }
            set
            {
                _searchValue = value;
                OnPropertyChanged();
            }
        }

        
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public BookManageViewModel()
        {
            setDefault();
            SaveCommand = new RelayCommand<Button>((p) =>
            {
                if (IsClick || IsEnable)
                {

                    if (string.IsNullOrEmpty(Ten) || NgayXuatBan < DateTime.Parse("01/01/1900")
                    || NgayXuatBan == null || SoLuong < 0
                    || SelectedAuthor == null || SelectedBookType == null)
                    {

                        return false;
                    }
                    IsEnable = true;
                    return true;
                }

                return false;
            }, (p) =>
            {
                if (BaseViewModel.SelectedItem != null && !IsAdd)
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
                else if (IsAdd)
                {
                    int checkName = DataProvider.Ins.DB.Saches.Where(x => x.TenSach == Ten).Count();
                    int checkType = DataProvider.Ins.DB.TheLoais.Where(x => x.TenTheLoai == SelectedBookType.TenTheLoai).Count();
                    int checkAuthor = DataProvider.Ins.DB.TacGias.Where(x => x.Ten == SelectedAuthor.Ten).Count();
                    if (checkAuthor > 0 && checkName > 0 && checkType > 0)
                    {
                        MessageBox.Show("Không thể thêm mới sách đã tồn tại trong kho.");
                    }
                    else
                    {
                        var book = new Sach();
                        book.ID = Guid.NewGuid().ToString();
                        book.TenSach = Ten;
                        book.SoLuong = SoLuong;
                        book.Gia = Gia;
                        book.IDTacGia = SelectedAuthor.ID;
                        book.IDTheLoai = SelectedBookType.ID;
                        book.NgayXuatBan = NgayXuatBan;
                        book.TrangThai = 1;
                        DataProvider.Ins.DB.AddToSaches(book);
                        DataProvider.Ins.DB.SaveChanges();
                        setDefault();
                    }
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

            SearchCommand = new RelayCommand<TextBlock>((p) =>
            {
                if (SearchType == null)
                    return false;
                if (!SearchType.Contains("Tất cả") && string.IsNullOrEmpty(SearchValue))
                    return false;
                if (SearchType.Contains("Tất cả") && !string.IsNullOrEmpty(SearchValue))
                    return false;
                return true;
            }, (p) =>
            {
                switch (SearchType)
                {
                    case "Theo tên":
                        switch (BookStatusSearch)
                        {
                            case "Tất cả":
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.Saches.Where(x => x.TenSach == SearchValue));
                                break;

                            case "Được lưu hành":
                                List.Clear();
                                List = loadList(List, 
                                    DataProvider.Ins.DB.Saches.Where(x => x.TenSach == SearchValue && x.TrangThai == 1));
                                break;

                            case "Đang cho mượn":
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.Saches.Where(x => x.TenSach == SearchValue && x.TrangThai == 2));
                                break;

                            case "Hư hỏng / Mất":
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.Saches.Where(x => x.TenSach == SearchValue && x.TrangThai == 3));
                                break;
                        }
                        break;

                    case "Theo thể loại":
                        switch (BookStatusSearch)
                        {
                            case "Tất cả":
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.Saches.Where(x => x.TheLoai.TenTheLoai == SearchValue));
                                break;

                            case "Được lưu hành":
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.Saches.Where(x => x.TheLoai.TenTheLoai == SearchValue && x.TrangThai == 1));
                                break;

                            case "Đang cho mượn":
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.Saches.Where(x => x.TheLoai.TenTheLoai == SearchValue && x.TrangThai == 2));
                                break;

                            case "Hư hỏng / Mất":
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.Saches.Where(x => x.TheLoai.TenTheLoai == SearchValue && x.TrangThai == 3));
                                break;
                        }
                        break;

                    case "Theo tác giả":
                        switch (BookStatusSearch)
                        {
                            case "Tất cả":
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.Saches.Where(x => x.TacGia.Ten == SearchValue));
                                break;

                            case "Được lưu hành":
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.Saches.Where(x => x.TacGia.Ten == SearchValue && x.TrangThai == 1));
                                break;

                            case "Đang cho mượn":
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.Saches.Where(x => x.TacGia.Ten == SearchValue && x.TrangThai == 2));
                                break;

                            case "Hư hỏng / Mất":
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.Saches.Where(x => x.TacGia.Ten == SearchValue && x.TrangThai == 3));
                                break;
                        }
                        break;

                    case "Theo năm":
                        switch (BookStatusSearch)
                        {
                            case "Tất cả":
                                try
                                {
                                    int year = Int32.Parse(SearchValue);
                                    List.Clear();
                                    List = loadList(List, DataProvider.Ins.DB.Saches.Where(x => x.NgayXuatBan.Year == year));
                                }
                                catch(Exception e)
                                {
                                    MessageBox.Show("Năm xuất bản phải là số.");
                                    break;
                                }
                                break;

                            case "Được lưu hành":
                                try
                                {
                                    int year = Int32.Parse(SearchValue);
                                    List.Clear();
                                    List = loadList(List, DataProvider.Ins.DB.Saches.Where(x => x.NgayXuatBan.Year == year && x.TrangThai == 1));
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Năm xuất bản phải là số.");
                                    break;
                                }
                                break;

                            case "Đang cho mượn":
                                try
                                {
                                    int year = Int32.Parse(SearchValue);
                                    List.Clear();
                                    List = loadList(List, DataProvider.Ins.DB.Saches.Where(x => x.NgayXuatBan.Year == year && x.TrangThai == 2));
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Năm xuất bản phải là số.");
                                    break;
                                }
                                break;

                            case "Hư hỏng / Mất":
                                try
                                {
                                    int year = Int32.Parse(SearchValue);
                                    List.Clear();
                                    List = loadList(List, DataProvider.Ins.DB.Saches.Where(x => x.NgayXuatBan.Year == year && x.TrangThai == 3));
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Năm xuất bản phải là số.");
                                    break;
                                }
                                break;
                        }
                        break;

                    case "Theo giá":
                        switch (BookStatusSearch)
                        {
                            case "Tất cả":
                                try
                                {
                                    float price = Int32.Parse(SearchValue);
                                    List.Clear();
                                    List = loadList(List, DataProvider.Ins.DB.Saches.Where(x => x.Gia == price));
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Giá tiền phải là số.");
                                    break;
                                }
                                break;

                            case "Được lưu hành":
                                try
                                {
                                    float price = Int32.Parse(SearchValue);
                                    List.Clear();
                                    List = loadList(List, DataProvider.Ins.DB.Saches.Where(x => x.Gia == price && x.TrangThai == 1));
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Năm xuất bản phải là số.");
                                    break;
                                }
                                break;

                            case "Đang cho mượn":
                                try
                                {
                                    float price = Int32.Parse(SearchValue);
                                    List.Clear();
                                    List = loadList(List, DataProvider.Ins.DB.Saches.Where(x => x.Gia == price && x.TrangThai == 2));
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Năm xuất bản phải là số.");
                                    break;
                                }
                                break;

                            case "Hư hỏng / Mất":
                                try
                                {
                                    float price = Int32.Parse(SearchValue);
                                    List.Clear();
                                    List = loadList(List, DataProvider.Ins.DB.Saches.Where(x => x.Gia == price && x.TrangThai == 3));
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Năm xuất bản phải là số.");
                                    break;
                                }
                                break;
                        }
                        break;
                    
                    default:
                        switch (BookStatusSearch)
                        {
                            case "Tất cả":
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.Saches);
                                break;

                            case "Được lưu hành":
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.Saches.Where(x => x.TrangThai == 1));
                                break;

                            case "Đang cho mượn":
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.Saches.Where(x => x.TrangThai == 2));
                                break;

                            case "Hư hỏng / Mất":
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.Saches.Where(x => x.TrangThai == 3));
                                break;
                        }
                        break;
                }
            });
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
            IsDelete = false;
            List = loadList(List, DataProvider.Ins.DB.Saches);
        }
        public ObservableCollection<BookList> loadList(ObservableCollection<BookList> list, object data)
        {
            list = new ObservableCollection<BookList>();
            int index = 1;
            var bookList = ((IEnumerable)data).Cast<object>().ToList();
            foreach (Sach book in bookList)
            {
                BookList item = new BookList();
                item.STT = index;
                item.Book = book;
                item.HoTenTacGia = book.TacGia.Ten;
                list.Add(item);
                index++;
            }
            return list;
        }
    }
}
