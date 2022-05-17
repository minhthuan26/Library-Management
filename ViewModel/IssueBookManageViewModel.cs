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
    public class IssueBookManageViewModel : BaseViewModel
    {
        private ObservableCollection<IssueBookList> _list;
        public ObservableCollection<IssueBookList> List { get { return _list; } set { _list = value; OnPropertyChanged(); } }
        
        private IssueBookList _selectedItem;
        public IssueBookList SelectedItem
        {
            get
            {
                IsClick = false;
                return (IssueBookList)_selectedItem;
            }
            set
            {
                _selectedItem = value;
                if (SelectedItem != null)
                    BaseViewModel.SelectedItem = SelectedItem;
                OnPropertyChanged();
            }
        }
        private List<string> _issueStatusSearchList = new List<string>()
        {
            "Tất cả",
            "Chưa trả sách",
            "Đã trả sách"
        };
        public List<string> IssueStatusSearchList
        {
            get
            {
                return _issueStatusSearchList;
            }
        }

        private List<string> _searchTypeList = new List<string>()
        {
            "Tất cả",
            "Theo tên người lập",
            "Theo tên khách hàng",
            "Theo năm"
        };
        public List<string> SearchTypeList
        {
            get
            {
                return _searchTypeList;
            }
        }
        private string _issueStatusSearch;
        public string IssueStatusSearch
        {
            get
            {
                if (_issueStatusSearch == null)
                    _issueStatusSearch = IssueStatusSearchList[0];
                return _issueStatusSearch;
            }
            set
            {
                _issueStatusSearch = value;
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
        public ICommand SeeDetailCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public IssueBookManageViewModel()
        {
            setDefault();

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
                    case "Theo tên người lập":
                        switch (IssueStatusSearch)
                        {
                            case "Tất cả":
                                setDefault();
                                List.Clear();
                                List = loadList(List, 
                                    DataProvider.Ins.DB.PhieuMuons.Where(x => x.NhanVien.HoVaTen == SearchValue),
                                    DataProvider.Ins.DB.ChiTietPhieuMuons.Where(x => x.PhieuMuon.NhanVien.HoVaTen.Contains(SearchValue)));
                                break;

                            case "Chưa trả sách":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.PhieuMuons.Where(x => x.NhanVien.HoVaTen.Contains(SearchValue) && x.TrangThai == 1),
                                    DataProvider.Ins.DB.ChiTietPhieuMuons.Where(x => x.PhieuMuon.NhanVien.HoVaTen.Contains(SearchValue)));
                                break;

                            case "Đã trả sách":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.PhieuMuons.Where(x => x.NhanVien.HoVaTen.Contains(SearchValue) && x.TrangThai == 0),
                                    DataProvider.Ins.DB.ChiTietPhieuMuons.Where(x => x.PhieuMuon.NhanVien.HoVaTen.Contains(SearchValue)));
                                break;
                        }
                        break;

                    case "Theo tên khách hàng":
                        switch (IssueStatusSearch)
                        {
                            case "Tất cả":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.PhieuMuons.Where(x => x.KhachHang.HoVaTen.Contains(SearchValue)),
                                    DataProvider.Ins.DB.ChiTietPhieuMuons.Where(x => x.PhieuMuon.KhachHang.HoVaTen.Contains(SearchValue)));
                                break;

                            case "Chưa trả sách":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.PhieuMuons.Where(x => x.KhachHang.HoVaTen.Contains(SearchValue) && x.TrangThai == 1),
                                    DataProvider.Ins.DB.ChiTietPhieuMuons.Where(x => x.PhieuMuon.KhachHang.HoVaTen.Contains(SearchValue)));
                                break;

                            case "Đã trả sách":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.PhieuMuons.Where(x => x.KhachHang.HoVaTen.Contains(SearchValue) && x.TrangThai == 0),
                                    DataProvider.Ins.DB.ChiTietPhieuMuons.Where(x => x.PhieuMuon.KhachHang.HoVaTen.Contains(SearchValue)));
                                break;
                        }
                        break;

                    case "Theo năm":
                        switch (IssueStatusSearch)
                        {
                            case "Tất cả":
                                try
                                {
                                    setDefault();
                                    int year = Int32.Parse(SearchValue);
                                    List.Clear();
                                    List = loadList(List,
                                    DataProvider.Ins.DB.PhieuMuons.Where(x => x.NgayLapPhieu.Year == year),
                                    DataProvider.Ins.DB.ChiTietPhieuMuons.Where(x => x.PhieuMuon.NgayLapPhieu.Year == year));
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Năm xuất bản phải là số.");
                                    break;
                                }
                                break;

                            case "Được lưu hành":
                                try
                                {
                                    setDefault();
                                    int year = Int32.Parse(SearchValue);
                                    List.Clear();
                                    List = loadList(List,
                                    DataProvider.Ins.DB.PhieuMuons.Where(x => x.NgayLapPhieu.Year == year && x.TrangThai == 1),
                                    DataProvider.Ins.DB.ChiTietPhieuMuons.Where(x => x.PhieuMuon.NgayLapPhieu.Year == year));
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
                                    setDefault();
                                    int year = Int32.Parse(SearchValue);
                                    List.Clear();
                                    List = loadList(List,
                                    DataProvider.Ins.DB.PhieuMuons.Where(x => x.NgayLapPhieu.Year == year && x.TrangThai == 0),
                                    DataProvider.Ins.DB.ChiTietPhieuMuons.Where(x => x.PhieuMuon.NgayLapPhieu.Year == year));
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
                        switch (IssueStatusSearch)
                        {
                            case "Tất cả":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.PhieuMuons,
                                    DataProvider.Ins.DB.ChiTietPhieuMuons);
                                break;

                            case "Chưa trả sách":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.PhieuMuons.Where(x => x.TrangThai == 1),
                                    DataProvider.Ins.DB.ChiTietPhieuMuons.Where(x => x.PhieuMuon.TrangThai == 1));
                                break;

                            case "Đã trả sách":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.PhieuMuons.Where(x => x.TrangThai == 0),
                                    DataProvider.Ins.DB.ChiTietPhieuMuons.Where(x => x.PhieuMuon.TrangThai == 0));
                                break;

                        }
                        break;
                }
            });

        }

        public void setDefault()
        {
            BaseViewModel.SelectedItem = null;
            IsEnable = false;
            IsClick = false;
            IsAdd = false;
            IsDelete = false;
            List = loadList(List, DataProvider.Ins.DB.PhieuMuons, DataProvider.Ins.DB.ChiTietPhieuMuons);
        }
        public ObservableCollection<IssueBookList> loadList(ObservableCollection<IssueBookList> list, object data1, object data2)
        {
            list = new ObservableCollection<IssueBookList>();
            int index = 1;
            var issueBookList = ((IEnumerable)data1).Cast<object>().ToArray();
            var issueDetailsList = ((IEnumerable)data2).Cast<object>().ToArray();
            for (int i=0; i<issueBookList.Length; i++)
            {
                IssueBookList item = new IssueBookList();
                item.STT = index;
                item.IssueBook = (PhieuMuon)issueBookList[i];
                item.DetailIssueBook = (ChiTietPhieuMuon)issueDetailsList[i];
                list.Add(item);
                index++;
            }
            return list;
        }
    }
}

