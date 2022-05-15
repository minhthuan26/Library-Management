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

    public class StaffManageViewModel : BaseViewModel
    {
        private ObservableCollection<StaffList> _list;
        public ObservableCollection<StaffList> List { get { return _list; } set { _list = value; OnPropertyChanged(); } }
        private ObservableCollection<ChucVu> _roleList;
        public ObservableCollection<ChucVu> RoleList { get { return _roleList; } set { _roleList = value; OnPropertyChanged(); } }
        private string _staffName;
        public string StaffName { get { return _staffName; } set { _staffName = value; OnPropertyChanged(); } }
        private string _phoneNumber;
        public string PhoneNumber { get { return _phoneNumber; } set { _phoneNumber = value; OnPropertyChanged(); } }
        private string _address;
        public string Address { get { return _address; } set { _address = value; OnPropertyChanged(); } }
        private DateTime _dateOfBirth;
        public DateTime DateOfBirth { get { return _dateOfBirth; } set { _dateOfBirth = value; OnPropertyChanged(); } }
        private ChucVu _selectedRole;
        public ChucVu SelectedRole
        {
            get
            {
                return (ChucVu)_selectedRole;
            }
            set
            {
                _selectedRole = value;
                OnPropertyChanged();
            }
        }

        private StaffList _selectedItem;
        public StaffList SelectedItem
        {
            get
            {
                IsClick = false;
                return (StaffList)_selectedItem;
            }
            set
            {
                _selectedItem = value;
                if (SelectedItem != null)
                {
                    BaseViewModel.SelectedItem = SelectedItem;
                    IsEnable = false;
                    StaffName = SelectedItem.Staff.HoVaTen;
                    Address = SelectedItem.Staff.DiaChi;
                    PhoneNumber = SelectedItem.Staff.SoDienThoai;
                    SelectedRole = SelectedItem.Staff.ChucVu;
                    DateOfBirth = SelectedItem.Staff.NgaySinh;
                }
                if (SelectedItem == null)
                {
                    StaffName = default;
                    Address = default;
                    PhoneNumber = default;
                    SelectedRole = default;
                    DateOfBirth = DateTime.Now;
                }
                OnPropertyChanged();
            }
        }

        private List<string> _searchTypeList = new List<string>()
        {
            "Tất cả",
            "Theo họ tên",
            "Theo số điện thoại",
            "Theo địa chỉ",
            "Theo năm sinh",
            "Theo chức vụ"
        };
        public List<string> SearchTypeList
        {
            get
            {
                return _searchTypeList;
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
        public StaffManageViewModel()
        {
            setDefault();
            SaveCommand = new RelayCommand<Button>((p) =>
            {
                if (IsClick)
                {

                    if (string.IsNullOrEmpty(StaffName) || DateOfBirth < DateTime.Parse("01/01/1900")
                        || string.IsNullOrEmpty(PhoneNumber) || SelectedRole == null 
                        || string.IsNullOrEmpty(Address))
                    {

                        return false;
                    }
                    //IsEnable = true;
                    return true;
                }

                return false;
            }, (p) =>
            {
                if (BaseViewModel.SelectedItem != null && !IsAdd)
                {
                    var staff = DataProvider.Ins.DB.NhanViens.Where(x => x.ID == SelectedItem.Staff.ID).SingleOrDefault();
                    staff.HoVaTen = StaffName;
                    staff.DiaChi = Address;
                    staff.SoDienThoai = PhoneNumber;
                    staff.NgaySinh = DateOfBirth;
                    staff.ChucVu = SelectedRole;
                    DataProvider.Ins.DB.SaveChanges();
                    setDefault();
                }
                else if (IsAdd)
                {
                    var staff = new NhanVien();
                    staff.ID = Guid.NewGuid().ToString();
                    staff.HoVaTen = StaffName;
                    staff.DiaChi = Address;
                    staff.SoDienThoai = PhoneNumber;
                    staff.NgaySinh = DateOfBirth;
                    staff.ChucVu = SelectedRole;
                    DataProvider.Ins.DB.AddToNhanViens(staff);
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
                    case "Theo họ tên":
                        setDefault();
                        List.Clear();
                        List = loadList(List, DataProvider.Ins.DB.NhanViens.Where(x => x.HoVaTen.Contains(SearchValue)));
                        break;

                    case "Theo số điện thoại":
                        setDefault();
                        List.Clear();
                        List = loadList(List, DataProvider.Ins.DB.NhanViens.Where(x => x.SoDienThoai.Contains(SearchValue)));
                        break;

                    case "Theo địa chỉ":
                        setDefault();
                        List.Clear();
                        List = loadList(List, DataProvider.Ins.DB.NhanViens.Where(x => x.DiaChi.Contains(SearchValue)));
                        break;

                    case "Theo năm sinh":
                        try
                        {
                            setDefault();
                            int year = Int32.Parse(SearchValue);
                            List.Clear();
                            List = loadList(List, DataProvider.Ins.DB.NhanViens.Where(x => x.NgaySinh.Year == year));
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Năm sinh phải là số.");
                            break;
                        }
                        break;

                    case "Theo chức vụ":
                        setDefault();
                        List.Clear();
                        List = loadList(List, DataProvider.Ins.DB.NhanViens.Where(x => x.ChucVu.TenChucVu.Contains(SearchValue)));
                        break;

                    default:
                        setDefault();
                        break;
                }
            });
        }

        public void setDefault()
        {
            RoleList = new ObservableCollection<ChucVu>(DataProvider.Ins.DB.ChucVus);
            BaseViewModel.SelectedItem = null;
            this.SelectedItem = null;
            IsEnable = false;
            IsClick = false;
            IsAdd = false;
            IsDelete = false;
            List = loadList(List, DataProvider.Ins.DB.NhanViens);
        }
        public ObservableCollection<StaffList> loadList(ObservableCollection<StaffList> list, object data)
        {
            list = new ObservableCollection<StaffList>();
            int index = 1;
            var staffList = ((IEnumerable)data).Cast<object>().ToList();
            foreach (NhanVien nhanVien in staffList)
            {
                var item = new StaffList();
                item.STT = index;
                item.Staff = nhanVien;
                list.Add(item);
                index++;
            }
            return list;
        }
    }
}
