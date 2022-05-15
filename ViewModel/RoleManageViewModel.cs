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
    public class RoleManageViewModel : BaseViewModel
    {
        private ObservableCollection<RoleList> _list;
        public ObservableCollection<RoleList> List { get { return _list; } set { _list = value; OnPropertyChanged(); } }
        private string _ten;
        public string Ten { get { return _ten; } set { _ten = value; OnPropertyChanged(); } }
        private RoleList _selectedItem;
        public RoleList SelectedItem
        {
            get
            {
                IsClick = false;
                return (RoleList)_selectedItem;
            }
            set
            {
                _selectedItem = value;
                if (SelectedItem != null)
                {
                    BaseViewModel.SelectedItem = SelectedItem;
                    IsEnable = false;
                    Ten = SelectedItem.Role.TenChucVu;
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

        public RoleManageViewModel()
        {
            setDefault();
            SaveCommand = new RelayCommand<Button>((p) =>
            {
                if (IsClick)
                {
                    int count = DataProvider.Ins.DB.ChucVus.Where(x => x.TenChucVu == Ten).Count();
                    if (string.IsNullOrEmpty(Ten) || count > 0)
                    {
                        return false;
                    }
                    return true;
                }

                return false;
            }, (p) =>
            {
                if (BaseViewModel.SelectedItem != null && !IsAdd)
                {
                    var role = DataProvider.Ins.DB.ChucVus.Where(x => x.ID == SelectedItem.Role.ID).SingleOrDefault();
                    role.TenChucVu = Ten;
                    DataProvider.Ins.DB.SaveChanges();
                    setDefault();
                }
                else if (IsAdd)
                {
                    var role = new ChucVu();
                    role.TenChucVu = Ten;
                    DataProvider.Ins.DB.AddToChucVus(role);
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
                if (IsClick)
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
                    DataProvider.Ins.DB.ChucVus.Where(x => x.TenChucVu.Contains(SearchValue)));
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
            List = loadList(List, DataProvider.Ins.DB.ChucVus);
        }
        public ObservableCollection<RoleList> loadList(ObservableCollection<RoleList> list, object data)
        {
            list = new ObservableCollection<RoleList>();
            int index = 1;
            var roleList = ((IEnumerable)data).Cast<object>().ToList();
            foreach (ChucVu role in roleList)
            {
                var item = new RoleList();
                item.STT = index;
                item.Role = role;
                list.Add(item);
                index++;
            }
            return list;
        }
    }
}
