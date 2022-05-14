using QuanLyThuVien.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyThuVien.ViewModel
{
    public class AuthorManageViewModel : BaseViewModel
    {
        private ObservableCollection<AuthorList> _list;
        public ObservableCollection<AuthorList> List { get { return _list; } set { _list = value; OnPropertyChanged(); } }
        private string _ten;
        public string Ten { get { return _ten; } set { _ten = value; OnPropertyChanged(); } }
        private AuthorList _selectedItem;
        public AuthorList SelectedItem
        {
            get
            {
                IsClick = false;
                return (AuthorList)_selectedItem;
            }
            set
            {
                _selectedItem = value;
                if (SelectedItem != null)
                {
                    BaseViewModel.SelectedItem = SelectedItem;
                    IsEnable = false;
                    Ten = SelectedItem.Author.Ten;
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

        public AuthorManageViewModel()
        {
            setDefault();
            SaveCommand = new RelayCommand<Button>((p) =>
            {
                if (IsClick || IsEnable)
                {
                    int count = DataProvider.Ins.DB.TacGias.Where(x => x.Ten == Ten).Count();
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
                    var author = DataProvider.Ins.DB.TacGias.Where(x => x.ID == SelectedItem.Author.ID).SingleOrDefault();
                    author.Ten = Ten;
                    DataProvider.Ins.DB.SaveChanges();
                    setDefault();
                }
                else if (IsAdd)
                {
                    var author = new TacGia();
                    author.Ten = Ten;
                    DataProvider.Ins.DB.AddToTacGias(author);
                    var a = DataProvider.Ins.DB.SaveChanges();
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
                setDefault();
                List.Clear();
                List = loadList(List,
                    DataProvider.Ins.DB.TacGias.Where(x => x.Ten == SearchValue));
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
            List = loadList(List, DataProvider.Ins.DB.TacGias);
        }
        public ObservableCollection<AuthorList> loadList(ObservableCollection<AuthorList> list, object data)
        {
            list = new ObservableCollection<AuthorList>();
            int index = 1;
            var authorList = ((IEnumerable)data).Cast<object>().ToList();
            foreach (TacGia author in authorList)
            {
                var item = new AuthorList();
                item.STT = index;
                item.Author = author;
                list.Add(item);
                index++;
            }
            return list;
        }
    }
}
