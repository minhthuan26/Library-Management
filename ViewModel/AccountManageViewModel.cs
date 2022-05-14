
using MaHoa;
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
    public class AccountManageViewModel : BaseViewModel
    {
        private ObservableCollection<TaiKhoan> _list;
        public ObservableCollection<TaiKhoan> List { get { return _list; } set { _list = value; OnPropertyChanged(); } }
        private ObservableCollection<ChucVu> _roleList;
        public ObservableCollection<ChucVu> RoleList { get { return _roleList; } set { _roleList = value; OnPropertyChanged(); } }
        private ObservableCollection<NhanVien> _staffList;
        public ObservableCollection<NhanVien> StaffList { get { return _staffList; } set { _staffList = value; OnPropertyChanged(); } }
        private string _accountName;
        public string AccountName { get { return _accountName; } set { _accountName = value; OnPropertyChanged(); } }

        private string _password;
        public string Password { get { return _password; } set { _password = value; OnPropertyChanged(); } }

        private ChucVu _selectedRole;
        public ChucVu SelectedRole
        {
            get
            {
                IsClick = false;
                return (ChucVu)_selectedRole;
            }
            set
            {
                _selectedRole = value;
                OnPropertyChanged();
            }
        }

        private NhanVien _selectedStaff;
        public NhanVien SelectedStaff
        {
            get
            {
                IsClick = false;
                return (NhanVien)_selectedStaff;
            }
            set
            {
                _selectedStaff = value;
                OnPropertyChanged();
            }
        }

        private List<string> _accountStatusSearchList = new List<string>()
        {
            "Tất cả",
            "Được hoạt động",
            "Đang bị khoá"
        };
        public List<string> AccountStatusSearchList
        {
            get
            {
                return _accountStatusSearchList;
            }
        }
        private List<string> _searchTypeList = new List<string>()
        {
            "Tất cả",
            "Theo tên",
            "Theo chức vụ",
            "Theo nhân viên"
        };
        public List<string> SearchTypeList
        {
            get
            {
                return _searchTypeList;
            }
        }

        private string _accountStatusSearch;
        public string AccountStatusSearch
        {
            get
            {
                if (_accountStatusSearch == null)
                    _accountStatusSearch = AccountStatusSearchList[0];
                return _accountStatusSearch;
            }
            set
            {
                _accountStatusSearch = value;
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
        private TaiKhoan _selectedItem;
        public TaiKhoan SelectedItem
        {
            get
            {
                IsClick = false;
                return (TaiKhoan)_selectedItem;
            }
            set
            {
                _selectedItem = value;
                if (SelectedItem != null)
                {
                    BaseViewModel.SelectedItem = SelectedItem;
                    IsEnable = false;
                    AccountName = SelectedItem.TenTaiKhoan;
                    Password = SelectedItem.MatKhau;
                    SelectedRole = SelectedItem.ChucVu;
                    SelectedStaff = SelectedItem.NhanVien;
                }
                if (SelectedItem == null)
                {
                    AccountName = default;
                    Password = default;
                    SelectedRole = default;
                    SelectedStaff = default;
                }
                OnPropertyChanged();
            }
        }
        

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public AccountManageViewModel()
        {
            setDefault();
            SaveCommand = new RelayCommand<Button>((p) =>
            {
                if (IsClick || IsEnable)
                {

                    if (string.IsNullOrEmpty(AccountName) ||
                        string.IsNullOrEmpty(Password) ||
                        SelectedStaff == null ||
                        SelectedRole == null)
                    {
                        return false;
                    }
                    if (DataProvider.Ins.DB.TaiKhoans.Where(x => x.TenTaiKhoan == AccountName).Count() > 0)
                        return false;
                    IsEnable = true;
                    return true;
                }

                return false;
            }, (p) =>
            {
                if (BaseViewModel.SelectedItem != null && !IsAdd)
                {
                    var taiKhoan = DataProvider.Ins.DB.TaiKhoans.Where(x => x.ID == SelectedItem.ID).SingleOrDefault();
                    taiKhoan.TenTaiKhoan = AccountName;
                    taiKhoan.MatKhau = Cryption.Vigenere.Encode(Password, AccountName);
                    taiKhoan.ChucVu = SelectedRole;
                    taiKhoan.NhanVien = SelectedStaff;
                    DataProvider.Ins.DB.SaveChanges();
                    setDefault();
                }
                else if (IsAdd)
                {
                    int checkName = DataProvider.Ins.DB.TaiKhoans.Where(x => x.TenTaiKhoan == AccountName).Count();
                    if (checkName > 0)
                    {
                        MessageBox.Show("Không thể thêm mới, tài khoản đã tồn tại.");
                    }
                    else
                    {
                        var taiKhoan = new TaiKhoan();
                        taiKhoan.TenTaiKhoan = AccountName;
                        taiKhoan.MatKhau = Cryption.Vigenere.Encode(Password, taiKhoan.ID.ToString());
                        taiKhoan.ChucVu = SelectedRole;
                        taiKhoan.TrangThai = 1;
                        taiKhoan.NhanVien = SelectedStaff;
                        DataProvider.Ins.DB.AddToTaiKhoans(taiKhoan);
                        DataProvider.Ins.DB.SaveChanges();
                        setDefault();
                    }
                }
                else
                {
                    setDefault();
                }
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
                        switch (AccountStatusSearch)
                        {
                            case "Tất cả":
                                setDefault();
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.TaiKhoans.Where(x => x.TenTaiKhoan == SearchValue));
                                break;

                            case "Được hoạt động":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.TaiKhoans.Where(x => x.TenTaiKhoan == SearchValue && x.TrangThai == 1));
                                break;

                            case "Đang bị khoá":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.TaiKhoans.Where(x => x.TenTaiKhoan == SearchValue && x.TrangThai == 0));
                                break;
                        }
                        break;

                    case "Theo chức vụ":
                        switch (AccountStatusSearch)
                        {
                            case "Tất cả":
                                setDefault();
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.TaiKhoans.Where(x => x.ChucVu.TenChucVu == SearchValue));
                                break;

                            case "Được hoạt động":
                                setDefault();
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.TaiKhoans.Where(x => x.ChucVu.TenChucVu == SearchValue && x.TrangThai == 1));
                                break;

                            case "Đang bị khoá":
                                setDefault();
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.TaiKhoans.Where(x => x.ChucVu.TenChucVu == SearchValue && x.TrangThai == 0));
                                break;
                        }
                        break;

                    case "Theo nhân viên":
                        switch (AccountStatusSearch)
                        {
                            case "Tất cả":
                                setDefault();
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.TaiKhoans.Where(x => x.NhanVien.HoVaTen == SearchValue));
                                break;

                            case "Được hoạt động":
                                setDefault();
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.TaiKhoans.Where(x => x.NhanVien.HoVaTen == SearchValue && x.TrangThai == 1));
                                break;

                            case "Đang bị khoá":
                                setDefault();
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.TaiKhoans.Where(x => x.NhanVien.HoVaTen == SearchValue && x.TrangThai == 0));
                                break;
                        }
                        break;

                    default:
                        switch (AccountStatusSearch)
                        {
                            case "Tất cả":
                                setDefault();
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.TaiKhoans);
                                break;

                            case "Được lưu hành":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.TaiKhoans.Where(x => x.TrangThai == 1));
                                break;

                            case "Đang bị khoá":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.TaiKhoans.Where(x => x.TrangThai == 0));
                                break;
                        }
                        break;
                }
            });
        }
        public void setDefault()
        {
            RoleList = new ObservableCollection<ChucVu>(DataProvider.Ins.DB.ChucVus);
            StaffList = new ObservableCollection<NhanVien>(DataProvider.Ins.DB.NhanViens);
            BaseViewModel.SelectedItem = null;
            this.SelectedItem = null;
            IsEnable = false;
            IsClick = false;
            IsAdd = false;
            IsDelete = false;
            List = loadList(List, DataProvider.Ins.DB.TaiKhoans);
        }
        public ObservableCollection<TaiKhoan> loadList(ObservableCollection<TaiKhoan> list, object data)
        {
            list = new ObservableCollection<TaiKhoan>();
            var accountList = ((IEnumerable)data).Cast<object>().ToList();
            foreach (TaiKhoan taiKhoan in accountList)
            {
                list.Add(taiKhoan);
            }
            return list;
        }
    }
}
