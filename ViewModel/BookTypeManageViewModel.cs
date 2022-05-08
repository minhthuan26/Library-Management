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
    public class BookTypeManageViewModel : BaseViewModel
    {
        private ObservableCollection<BookTypeList> _list;
        public ObservableCollection<BookTypeList> List { get { return _list; } set { _list = value; OnPropertyChanged(); } }
        private string _ten;
        public string Ten { get { return _ten; } set { _ten = value; OnPropertyChanged(); } }
        private BookTypeList _selectedItem;
        public BookTypeList SelectedItem
        {
            get
            {
                IsClick = false;
                return (BookTypeList)_selectedItem;
            }
            set
            {
                _selectedItem = value;
                if (SelectedItem != null)
                {
                    BaseViewModel.SelectedItem = SelectedItem;
                    IsEnable = false;
                    Ten = SelectedItem.BookType.TenTheLoai;
                }
                if (SelectedItem == null)
                {
                    Ten = default;
                }
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
        public ICommand RefreshCommand { get; set; }

        public BookTypeManageViewModel()
        {
            setDefault();
            SaveCommand = new RelayCommand<Button>((p) =>
            {
                if (IsClick || IsEnable)
                {
                    int count = DataProvider.Ins.DB.TheLoais.Where(x => x.TenTheLoai == Ten).Count();
                    if (string.IsNullOrEmpty(Ten) || count > 0)
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
                    var bookType = DataProvider.Ins.DB.TheLoais.Where(x => x.ID == SelectedItem.BookType.ID).SingleOrDefault();
                    bookType.TenTheLoai = Ten;
                    DataProvider.Ins.DB.SaveChanges();
                    setDefault();
                }
                else if (IsAdd)
                {
                    var bookType = new TheLoai();
                    bookType.TenTheLoai = Ten;
                    DataProvider.Ins.DB.AddToTheLoais(bookType);
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

            SearchCommand = new RelayCommand<TextBlock>((p) =>
            {
                if (string.IsNullOrEmpty(SearchValue))
                    return false;
                return true;
            }, (p) =>
            {
                List.Clear();
                List = loadList(List,
                    DataProvider.Ins.DB.TheLoais.Where(x => x.TenTheLoai == SearchValue));
            });

            RefreshCommand = new RelayCommand<TextBlock>((p) =>
            {
                return true;
            }, (p) =>
            {
                setDefault();
            });
        }

        public void setDefault()
        {
            BaseViewModel.SelectedItem = null;
            this.SelectedItem = null;
            IsEnable = false;
            IsClick = false;
            IsAdd = false;
            IsDelete = false;
            List = loadList(List, DataProvider.Ins.DB.TheLoais);
        }
        public ObservableCollection<BookTypeList> loadList(ObservableCollection<BookTypeList> list, object data)
        {
            list = new ObservableCollection<BookTypeList>();
            int index = 1;
            var bookTypeList = ((IEnumerable)data).Cast<object>().ToList();
            foreach (TheLoai bookType in bookTypeList)
            {
                BookTypeList item = new BookTypeList();
                item.STT = index;
                item.BookType = bookType;
                list.Add(item);
                index++;
            }
            return list;
        }
    }
}
