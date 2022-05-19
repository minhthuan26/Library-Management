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
    public class IndemnifyManageViewModel : BaseViewModel
    {
        private ObservableCollection<IndemnifyList> _list;
        public ObservableCollection<IndemnifyList> List { get { return _list; } set { _list = value; OnPropertyChanged(); } }

        private IndemnifyList _selectedItem;
        public IndemnifyList SelectedItem
        {
            get
            {
                IsClick = false;
                return (IndemnifyList)_selectedItem;
            }
            set
            {
                _selectedItem = value;
                if (SelectedItem != null)
                    BaseViewModel.SelectedItem = SelectedItem;
                OnPropertyChanged();
            }
        }
        private List<string> _indemnifyStatusSearchList = new List<string>()
        {
            "Tất cả",
            "Chưa thanh toán",
            "Đã thanh toán"
        };
        public List<string> IndemnifyStatusSearchList
        {
            get
            {
                return _indemnifyStatusSearchList;
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
        private string _indemnifyStatusSearch;
        public string IndemnifyStatusSearch
        {
            get
            {
                if (_indemnifyStatusSearch == null)
                    _indemnifyStatusSearch = IndemnifyStatusSearchList[0];
                return _indemnifyStatusSearch;
            }
            set
            {
                _indemnifyStatusSearch = value;
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
        public IndemnifyManageViewModel()
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
                        switch (IndemnifyStatusSearch)
                        {
                            case "Tất cả":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.PhieuBoiThuongs.Where(x => x.NhanVien.HoVaTen == SearchValue),
                                    DataProvider.Ins.DB.ChiTietPhieuBoiThuongs.Where(x => x.PhieuBoiThuong.NhanVien.HoVaTen.Contains(SearchValue)));
                                break;

                            case "Chưa thanh toán":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.PhieuBoiThuongs.Where(x => x.NhanVien.HoVaTen.Contains(SearchValue) && x.TrangThai == 1),
                                    DataProvider.Ins.DB.ChiTietPhieuBoiThuongs.Where(x => x.PhieuBoiThuong.NhanVien.HoVaTen.Contains(SearchValue)));
                                break;

                            case "Đã thanh toán":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.PhieuBoiThuongs.Where(x => x.NhanVien.HoVaTen.Contains(SearchValue) && x.TrangThai == 0),
                                    DataProvider.Ins.DB.ChiTietPhieuBoiThuongs.Where(x => x.PhieuBoiThuong.NhanVien.HoVaTen.Contains(SearchValue)));
                                break;
                        }
                        break;

                    case "Theo tên khách hàng":
                        switch (IndemnifyStatusSearch)
                        {
                            case "Tất cả":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.PhieuBoiThuongs.Where(x => x.PhieuMuon.KhachHang.HoVaTen.Contains(SearchValue)),
                                    DataProvider.Ins.DB.ChiTietPhieuBoiThuongs.Where(x => x.PhieuBoiThuong.PhieuMuon.KhachHang.HoVaTen.Contains(SearchValue)));
                                break;

                            case "Chưa thanh toán":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.PhieuBoiThuongs.Where(x => x.PhieuMuon.KhachHang.HoVaTen.Contains(SearchValue) && x.TrangThai == 1),
                                    DataProvider.Ins.DB.ChiTietPhieuBoiThuongs.Where(x => x.PhieuBoiThuong.PhieuMuon.KhachHang.HoVaTen.Contains(SearchValue)));
                                break;

                            case "Đã thanh toán":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.PhieuBoiThuongs.Where(x => x.PhieuMuon.KhachHang.HoVaTen.Contains(SearchValue) && x.TrangThai == 0),
                                    DataProvider.Ins.DB.ChiTietPhieuBoiThuongs.Where(x => x.PhieuBoiThuong.PhieuMuon.KhachHang.HoVaTen.Contains(SearchValue)));
                                break;
                        }
                        break;

                    case "Theo năm":
                        switch (IndemnifyStatusSearch)
                        {
                            case "Tất cả":
                                try
                                {
                                    setDefault();
                                    int year = Int32.Parse(SearchValue);
                                    List.Clear();
                                    List = loadList(List,
                                    DataProvider.Ins.DB.PhieuBoiThuongs.Where(x => x.NgayLapPhieu.Year == year),
                                    DataProvider.Ins.DB.ChiTietPhieuBoiThuongs.Where(x => x.PhieuBoiThuong.NgayLapPhieu.Year == year));
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
                                    DataProvider.Ins.DB.PhieuBoiThuongs.Where(x => x.NgayLapPhieu.Year == year && x.TrangThai == 1),
                                    DataProvider.Ins.DB.ChiTietPhieuBoiThuongs.Where(x => x.PhieuBoiThuong.NgayLapPhieu.Year == year));
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
                                    DataProvider.Ins.DB.PhieuBoiThuongs.Where(x => x.NgayLapPhieu.Year == year && x.TrangThai == 0),
                                    DataProvider.Ins.DB.ChiTietPhieuBoiThuongs.Where(x => x.PhieuBoiThuong.NgayLapPhieu.Year == year));
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
                        switch (IndemnifyStatusSearch)
                        {
                            case "Tất cả":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.PhieuBoiThuongs,
                                    DataProvider.Ins.DB.ChiTietPhieuBoiThuongs);
                                break;

                            case "Chưa thanh toán":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.PhieuBoiThuongs.Where(x => x.TrangThai == 1),
                                    DataProvider.Ins.DB.ChiTietPhieuBoiThuongs.Where(x => x.PhieuBoiThuong.TrangThai == 1));
                                break;

                            case "Đã thanh toán":
                                setDefault();
                                List.Clear();
                                List = loadList(List,
                                    DataProvider.Ins.DB.PhieuBoiThuongs.Where(x => x.TrangThai == 0),
                                    DataProvider.Ins.DB.ChiTietPhieuBoiThuongs.Where(x => x.PhieuBoiThuong.TrangThai == 0));
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
            List = loadList(List, DataProvider.Ins.DB.PhieuBoiThuongs, DataProvider.Ins.DB.ChiTietPhieuBoiThuongs);
        }
        public ObservableCollection<IndemnifyList> loadList(ObservableCollection<IndemnifyList> list, object data1, object data2)
        {
            list = new ObservableCollection<IndemnifyList>();
            int index = 1;
            var IndemnifyList = ((IEnumerable)data1).Cast<object>().ToArray();
            var IndemnifyDetailsList = ((IEnumerable)data2).Cast<object>().ToArray();
            for (int i = 0; i < IndemnifyList.Length; i++)
            {
                IndemnifyList item = new IndemnifyList();
                item.STT = index;
                item.IndemnifyBook = (PhieuBoiThuong)IndemnifyList[i];
                item.DetailIndemnifyBook = (ChiTietPhieuBoiThuong)IndemnifyDetailsList[i];
                list.Add(item);
                index++;
            }
            return list;
        }
    }
}
