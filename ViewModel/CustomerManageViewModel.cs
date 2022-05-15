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
    public class CustomerManageViewModel : BaseViewModel
    {
        private ObservableCollection<CustomerList> _list;
        public ObservableCollection<CustomerList> List { get { return _list; } set { _list = value; OnPropertyChanged(); } }
       private string _customerName;
        public string CustomerName { get { return _customerName; } set { _customerName = value; OnPropertyChanged(); } }
        private string _phoneNumber;
        public string PhoneNumber { get { return _phoneNumber; } set { _phoneNumber = value; OnPropertyChanged(); } }
        private string _address;
        public string Address { get { return _address; } set { _address = value; OnPropertyChanged(); } }
        private DateTime _dateOfBirth;
        public DateTime DateOfBirth { get { return _dateOfBirth; } set { _dateOfBirth = value; OnPropertyChanged(); } }
        private CustomerList _selectedItem;
        public CustomerList SelectedItem
        {
            get
            {
                IsClick = false;
                return (CustomerList)_selectedItem;
            }
            set
            {
                _selectedItem = value;
                if (SelectedItem != null)
                {
                    BaseViewModel.SelectedItem = SelectedItem;
                    IsEnable = false;
                    CustomerName = SelectedItem.Customer.HoVaTen;
                    Address = SelectedItem.Customer.DiaChi;
                    PhoneNumber = SelectedItem.Customer.SoDienThoai;
                    DateOfBirth = SelectedItem.Customer.NgaySinh;
                }
                if (SelectedItem == null)
                {
                    CustomerName = default;
                    Address = default;
                    PhoneNumber = default;
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
            "Theo năm sinh"
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

        private List<string> _customerStatusSearchList = new List<string>()
        {
            "Tất cả",
            "Được hoạt động",
            "Đang bị khoá"
        };
        public List<string> CustomerStatusSearchList
        {
            get
            {
                return _customerStatusSearchList;
            }
        }

        private string _customerStatusSearch;
        public string CustomerStatusSearch
        {
            get
            {
                if (_customerStatusSearch == null)
                    _customerStatusSearch = CustomerStatusSearchList[0];
                return _customerStatusSearch;
            }
            set
            {
                _customerStatusSearch = value;
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
        public CustomerManageViewModel()
        {
            setDefault();
            SaveCommand = new RelayCommand<Button>((p) =>
            {
                if (IsClick)
                {

                    if (string.IsNullOrEmpty(CustomerName) || DateOfBirth < DateTime.Parse("01/01/1900")
                        || string.IsNullOrEmpty(PhoneNumber) || string.IsNullOrEmpty(Address))
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
                    var customer = DataProvider.Ins.DB.KhachHangs.Where(x => x.ID == SelectedItem.Customer.ID).SingleOrDefault();
                    customer.HoVaTen = CustomerName;
                    customer.DiaChi = Address;
                    customer.SoDienThoai = PhoneNumber;
                    customer.NgaySinh = DateOfBirth;
                    DataProvider.Ins.DB.SaveChanges();
                    setDefault();
                }
                else if (IsAdd)
                {
                    var customer = new KhachHang();
                    customer.ID = Guid.NewGuid().ToString();
                    customer.HoVaTen = CustomerName;
                    customer.DiaChi = Address;
                    customer.SoDienThoai = PhoneNumber;
                    customer.NgaySinh = DateOfBirth;
                    DataProvider.Ins.DB.AddToKhachHangs(customer);
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

            SearchCommand = new RelayCommand<TextBox>((p) =>
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
                        switch (CustomerStatusSearch)
                        {
                            case "Tất cả":
                                setDefault();
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.KhachHangs.Where(x => x.HoVaTen.Contains(SearchValue)));
                                break;

                            case "Được hoạt động":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.KhachHangs.Where(x => x.HoVaTen.Contains(SearchValue) && x.TrangThai == 1));
                                break;

                            case "Đang bị khoá":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.KhachHangs.Where(x => x.HoVaTen.Contains(SearchValue) && x.TrangThai == 0));
                                break;
                        }
                        break;

                    case "Theo số điện thoại":
                        switch (CustomerStatusSearch)
                        {
                            case "Tất cả":
                                setDefault();
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.KhachHangs.Where(x => x.SoDienThoai.Contains(SearchValue)));
                                break;

                            case "Được hoạt động":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.KhachHangs.Where(x => x.SoDienThoai.Contains(SearchValue) && x.TrangThai == 1));
                                break;

                            case "Đang bị khoá":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.KhachHangs.Where(x => x.SoDienThoai.Contains(SearchValue) && x.TrangThai == 0));
                                break;
                        }
                        break;

                    case "Theo địa chỉ":
                        switch (CustomerStatusSearch)
                        {
                            case "Tất cả":
                                setDefault();
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.KhachHangs.Where(x => x.DiaChi.Contains(SearchValue)));
                                break;

                            case "Được hoạt động":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.KhachHangs.Where(x => x.DiaChi.Contains(SearchValue) && x.TrangThai == 1));
                                break;

                            case "Đang bị khoá":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.KhachHangs.Where(x => x.DiaChi.Contains(SearchValue) && x.TrangThai == 0));
                                break;
                        }
                        break;

                    case "Theo năm sinh":
                        switch (CustomerStatusSearch)
                        {
                            case "Tất cả":
                                try
                                {
                                    setDefault();
                                    int year = Int32.Parse(SearchValue);
                                    List.Clear();
                                    List = loadList(List, DataProvider.Ins.DB.KhachHangs.Where(x => x.NgaySinh.Year == year));
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Giá tiền phải là số.");
                                    break;
                                }
                                break;

                            case "Được hoạt động":
                                try
                                {
                                    setDefault();
                                    int year = Int32.Parse(SearchValue);
                                    List.Clear();
                                    List = loadList(List, DataProvider.Ins.DB.KhachHangs.Where(x => x.NgaySinh.Year == year && x.TrangThai == 1));
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Năm xuất bản phải là số.");
                                    break;
                                }
                                break;

                            case "Đang bị khoá":
                                try
                                {
                                    setDefault();
                                    int year = Int32.Parse(SearchValue);
                                    List.Clear();
                                    List = loadList(List, DataProvider.Ins.DB.KhachHangs.Where(x => x.NgaySinh.Year == year && x.TrangThai == 0));
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
                        switch (CustomerStatusSearch)
                        {
                            case "Tất cả":
                                setDefault();
                                List.Clear();
                                List = loadList(List, DataProvider.Ins.DB.KhachHangs);
                                break;

                            case "Được hoạt động":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.KhachHangs.Where(x => x.TrangThai == 1));
                                break;

                            case "Đang bị khoá":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.KhachHangs.Where(x => x.TrangThai == 0));
                                break;
                        }
                        break;
                }
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
            List = loadList(List, DataProvider.Ins.DB.KhachHangs);
        }
        public ObservableCollection<CustomerList> loadList(ObservableCollection<CustomerList> list, object data)
        {
            list = new ObservableCollection<CustomerList>();
            int index = 1;
            var customerList = ((IEnumerable)data).Cast<object>().ToList();
            foreach (KhachHang khachHang in customerList)
            {
                var item = new CustomerList();
                item.STT = index;
                item.Customer = khachHang;
                list.Add(item);
                index++;
            }
            return list;
        }
    }
}
