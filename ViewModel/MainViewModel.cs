using QuanLyThuVien.Model;
using QuanLyThuVien.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyThuVien.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private static BaseViewModel _currentView;
        public static BaseViewModel CurrentView { get { return _currentView; } set { _currentView = value; } }
        private BaseViewModel _selectView;
        public BaseViewModel SelectView { get { if (_selectView == null) _selectView = new GeneralManageViewModel(); return _selectView; } set { _selectView = value; OnPropertyChanged(); } }
        private string _viewTitle;
        public string ViewTitle { get { if (_viewTitle == null) _viewTitle = "Tổng quan"; return _viewTitle; } set { _viewTitle = value; OnPropertyChanged(); } }
        private static Button _currentBtn = new Button();
        public static Button CurrentBtn { get { return _currentBtn; } set { _currentBtn = value; } }
        private string _messageText;
        public string MessageText { get { return _messageText; } set { _messageText = value; OnPropertyChanged(); } }
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand ChangeViewCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand ConfirmReturnBookCommand { get; set; }
        public ICommand ConfirmIndemnifyBookCommand { get; set; }
        public ICommand BlockAccountCommand { get; set; }
        public ICommand BlockCustomerCommand { get; set; }
        public ICommand CreateIssueBookCommand { get; set; }
        public ICommand EditIssueBookCommand { get; set; }
        public ICommand SeeDetailIssueBookCommand { get; set; }
        public ICommand SeeDetailIndemnifyBookCommand { get; set; }
        public ICommand CompensationCommand { get; set; }
        public MainViewModel()
        {
            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (p == null)
                    return;
                p.Hide();
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
                if (loginWindow.DataContext == null)
                    return;
                var loginVM = loginWindow.DataContext as LoginViewModel;
                if (loginVM.IsLogin)
                {
                    //BookManageViewModel test = new BookManageViewModel(true);
                    GeneralManageViewModel generalManageVM = new GeneralManageViewModel();
                    p.Show();
                }
                else
                {
                    p.Close();
                }
            });

            ChangeViewCommand = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                string buttonName = p.Name.ToString();
                Dictionary<string, object> viewList = new Dictionary<string, object>()
                {
                    {"staffBtn", new StaffManageViewModel() },
                    {"bookBtn", new BookManageViewModel() },
                    {"customerBtn", new CustomerManageViewModel() },
                    {"issueBookBtn", new IssueBookManageViewModel() },
                    {"returnBookBtn", new ReturnBookManageViewModel() },
                    {"indemnifyBtn", new IndemnifyManageViewModel() },
                    {"accountBtn", new AccountManageViewModel() },
                    {"bookTypeBtn", new BookTypeManageViewModel() },
                    {"authorBtn", new AuthorManageViewModel() },
                    {"roleBtn", new RoleManageViewModel() },
                    {"statisticsBtn", new StatisticsManageViewModel() }
                };

                Dictionary<object, string> titleList = new Dictionary<object, string>()
                {
                    {new StaffManageViewModel(), "Quản lí nhân viên" },
                    {new BookManageViewModel(), "Quản lí sách" },
                    {new CustomerManageViewModel(), "Quản lí khách hàng" },
                    {new IssueBookManageViewModel(), "Quản lí mượn sách" },
                    {new ReturnBookManageViewModel(), "Quản lí trả sách" },
                    {new IndemnifyManageViewModel(), "Quản lí bồi thường" },
                    {new AccountManageViewModel(), "Quản lí tài khoản" },
                    {new BookTypeManageViewModel(), "Quản lí thể loại sách" },
                    {new AuthorManageViewModel(), "Quản lí tác giả" },
                    {new RoleManageViewModel(), "Quản lí chức vụ" },
                    {new GeneralManageViewModel(), "Tổng quan" },
                    {new StatisticsManageViewModel(), "Báo cáo thống kê" }
                };
                //MessageBox.Show(buttonName);
                if (buttonName != CurrentBtn.Name)
                {
                    foreach (var view in viewList)
                    {
                        if (view.Key == buttonName)
                        {
                            //MessageBox.Show("khac");
                            //CurrentBtn = p;
                            //MessageBox.Show(CurrentBtn.Name);
                            //break;
                            expandButton(CurrentBtn);
                            CurrentBtn = p;
                            expandButton(p);
                            CurrentView = SelectView = (BaseViewModel)view.Value;
                            foreach (var title in titleList)
                            {
                                if (title.Key.ToString() == SelectView.ToString())
                                {
                                    ViewTitle = title.Value;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                else
                {
                    IsEnable = false;
                    IsClick = false;
                    IsAdd = false;
                    IsDelete = false;
                    ViewTitle = null;
                    BaseViewModel.SelectedItem = null;
                    CurrentBtn = new Button();
                    expandButton(p);
                    SelectView = null;
                }
                
            });

            EditCommand = new RelayCommand<Button>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            { 
                IsClick = true;
                IsAdd = false;
            });

            AddCommand = new RelayCommand<Button>((p) =>
            {
                //if (SelectedItem == null)
                //    return false;
                return true;
            }, (p) =>
            {
                SelectedItem = null;
                IsClick = true;
                IsAdd = true;
            });

            DeleteCommand = new RelayCommand<Grid>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                var tmp = SelectedItem;
                switch (ViewTitle)
                {
                    case "Quản lí sách":
                        BookManageViewModel bookManageViewModel = (BookManageViewModel)SelectView;
                        BookList bookList = (BookList)SelectedItem;
                        Sach book = bookList.Book;
                        MessageText = "Đã xoá sách " + "\"" + book.TenSach + "\".";
                        DataProvider.Ins.DB.Saches.DeleteObject(book);
                        DataProvider.Ins.DB.SaveChanges();
                        bookManageViewModel.setDefault();
                        CurrentView = SelectView;
                        SelectView = null;
                        p.Visibility = Visibility.Hidden;
                        IsVisible = "Visible";
                        break;

                    case "Quản lí thể loại sách":
                        BookTypeManageViewModel bookTypeManageViewModel = (BookTypeManageViewModel)SelectView;
                        BookTypeList bookTypeList = (BookTypeList)SelectedItem;
                        TheLoai bookType = bookTypeList.BookType;
                        MessageText = "Đã xoá thể loại " + "\"" + bookType.TenTheLoai + "\".";
                        DataProvider.Ins.DB.TheLoais.DeleteObject(bookType);
                        DataProvider.Ins.DB.SaveChanges();
                        bookTypeManageViewModel.setDefault();
                        CurrentView = SelectView;
                        SelectView = null;
                        p.Visibility = Visibility.Hidden;
                        IsVisible = "Visible";
                        break;

                    case "Quản lí chức vụ":
                        RoleManageViewModel roleManageViewModel = (RoleManageViewModel)SelectView;
                        RoleList roleList = (RoleList)SelectedItem;
                        ChucVu role = roleList.Role;
                        MessageText = "Đã xoá chức vụ " + "\"" + role.TenChucVu + "\".";
                        DataProvider.Ins.DB.ChucVus.DeleteObject(role);
                        DataProvider.Ins.DB.SaveChanges();
                        roleManageViewModel.setDefault();
                        CurrentView = SelectView;
                        SelectView = null;
                        p.Visibility = Visibility.Hidden;
                        IsVisible = "Visible";
                        break;

                    case "Quản lí tác giả":
                        AuthorManageViewModel authorManageViewModel = (AuthorManageViewModel)SelectView;
                        AuthorList authorList = (AuthorList)SelectedItem;
                        TacGia author = authorList.Author;
                        MessageText = "Đã xoá tác giả " + "\"" + author.Ten + "\".";
                        DataProvider.Ins.DB.TacGias.DeleteObject(author);
                        DataProvider.Ins.DB.SaveChanges();
                        authorManageViewModel.setDefault();
                        CurrentView = SelectView;
                        SelectView = null;
                        p.Visibility = Visibility.Hidden;
                        IsVisible = "Visible";
                        break;

                    case "Quản lí mượn sách":
                        IssueBookManageViewModel issueBookManageViewModel = (IssueBookManageViewModel)SelectView;
                        IssueBookList issueBookList = (IssueBookList)SelectedItem;
                        var detailList = ((IEnumerable)DataProvider.Ins.DB.ChiTietPhieuMuons.Where(x => x.PhieuMuon.ID == issueBookList.IssueBook.ID)).Cast<object>().ToList();
                        foreach(ChiTietPhieuMuon chiTiet in detailList)
                        {
                            chiTiet.Sach.SoLuong++;
                            DataProvider.Ins.DB.ChiTietPhieuMuons.DeleteObject(chiTiet);
                        }
                        MessageText = "Đã xoá phiếu mượn có ID " + "\"" + issueBookList.IssueBook.ID + "\".";
                        DataProvider.Ins.DB.PhieuMuons.DeleteObject(issueBookList.IssueBook);
                        DataProvider.Ins.DB.SaveChanges();
                        issueBookManageViewModel.setDefault();
                        CurrentView = SelectView;
                        SelectView = null;
                        p.Visibility = Visibility.Hidden;
                        IsVisible = "Visible";
                        break;

                    case "Quản lí bồi thường":
                        IndemnifyManageViewModel indemnifyManageViewModel = (IndemnifyManageViewModel)SelectView;
                        IndemnifyList indemnifyList = (IndemnifyList)SelectedItem;
                        var detailIndemnifyList = ((IEnumerable)DataProvider.Ins.DB.ChiTietPhieuBoiThuongs.Where(x => x.PhieuBoiThuong.ID == indemnifyList.IndemnifyBook.ID)).Cast<object>().ToList();
                        foreach (ChiTietPhieuBoiThuong chiTiet in detailIndemnifyList)
                        {
                            chiTiet.Sach.SoLuong++;
                            DataProvider.Ins.DB.ChiTietPhieuBoiThuongs.DeleteObject(chiTiet);
                        }
                        MessageText = "Đã xoá phiếu bồi thường có ID " + "\"" + indemnifyList.IndemnifyBook.ID + "\".";
                        DataProvider.Ins.DB.PhieuBoiThuongs.DeleteObject(indemnifyList.IndemnifyBook);
                        DataProvider.Ins.DB.SaveChanges();
                        indemnifyManageViewModel.setDefault();
                        CurrentView = SelectView;
                        SelectView = null;
                        p.Visibility = Visibility.Hidden;
                        IsVisible = "Visible";
                        break;

                    case "Quản lí nhân viên":
                        StaffManageViewModel staffManageViewModel = (StaffManageViewModel)SelectView;
                        StaffList staffList = (StaffList)SelectedItem;
                        NhanVien staff = staffList.Staff;
                        MessageText = "Đã xoá nhân viên " + "\"" + staff.HoVaTen + "\".";
                        DataProvider.Ins.DB.NhanViens.DeleteObject(staff);
                        DataProvider.Ins.DB.SaveChanges();
                        staffManageViewModel.setDefault();
                        CurrentView = SelectView;
                        SelectView = null;
                        p.Visibility = Visibility.Hidden;
                        IsVisible = "Visible";
                        break;

                    case "Quản lí khách hàng":
                        CustomerManageViewModel customerManageViewModel = (CustomerManageViewModel)SelectView;
                        CustomerList customerList = (CustomerList)SelectedItem;
                        KhachHang khachHang = customerList.Customer;
                        DataProvider.Ins.DB.KhachHangs.DeleteObject(khachHang);
                        MessageText = "Đã xoá khách hàng " + "\"" + khachHang.HoVaTen + "\".";
                        DataProvider.Ins.DB.SaveChanges();
                        customerManageViewModel.setDefault();
                        CurrentView = SelectView;
                        SelectView = null;
                        p.Visibility = Visibility.Hidden;
                        IsVisible = "Visible";
                        break;

                    case "Quản lí tài khoản":
                        AccountManageViewModel accountManageViewModel = (AccountManageViewModel)SelectView;
                        TaiKhoan taiKhoan = (TaiKhoan)SelectedItem;
                        DataProvider.Ins.DB.TaiKhoans.DeleteObject(taiKhoan);
                        MessageText = "Đã xoá tài khoản " + "\"" + taiKhoan.TenTaiKhoan + "\".";
                        DataProvider.Ins.DB.SaveChanges();
                        accountManageViewModel.setDefault();
                        CurrentView = SelectView;
                        SelectView = null;
                        p.Visibility = Visibility.Hidden;
                        IsVisible = "Visible";
                        break;
                }
                
            });

            BlockAccountCommand = new RelayCommand<Grid>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                TaiKhoan taiKhoan = (TaiKhoan)SelectedItem;
                taiKhoan.TrangThai = taiKhoan.TrangThai == 1 ? 0:1;
                DataProvider.Ins.DB.SaveChanges();
                AccountManageViewModel accountManage = (AccountManageViewModel)SelectView;
                accountManage.setDefault();
                CurrentView = SelectView;
                SelectView = null;
                p.Visibility = Visibility.Hidden;
                MessageText = taiKhoan.TrangThai == 1 ?
                    "Đã mở khoá tài khoản " + "\"" + taiKhoan.TenTaiKhoan + "\"." 
                    : "Đã khoá tài khoản " + "\"" + taiKhoan.TenTaiKhoan + "\".";
                IsVisible = "Visible";
            });

            BlockCustomerCommand = new RelayCommand<Grid>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                CustomerList customerList = (CustomerList)SelectedItem;
                KhachHang khachHang = customerList.Customer;
                khachHang.TrangThai = khachHang.TrangThai == 1 ? 0 : 1;
                DataProvider.Ins.DB.SaveChanges();
                CustomerManageViewModel customerManage = (CustomerManageViewModel)SelectView;
                customerManage.setDefault();
                CurrentView = SelectView;
                SelectView = null;
                p.Visibility = Visibility.Hidden;
                MessageText = khachHang.TrangThai == 1 ?
                    "Đã mở khoá khách hàng " + "\"" + khachHang.HoVaTen + "\"."
                    : "Đã khoá khách hàng " + "\"" + khachHang.HoVaTen + "\".";
                IsVisible = "Visible";
            });

            ConfirmCommand = new RelayCommand<Grid>((p) =>
            {
                return true;
            }, (p) =>
            {
                SelectView = CurrentView;
                p.Visibility = Visibility.Visible;
                IsVisible = "Hidden";
            });

            CreateIssueBookCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {   
                IssueBookManageViewModel issueBook = (IssueBookManageViewModel)SelectView;
                CreateIssueWindow view = new CreateIssueWindow();
                view.ShowDialog();
            });

            CompensationCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                IssueBookList phieuMuon = (IssueBookList)SelectedItem;
                if (phieuMuon.IssueBook.TrangThai == 0)
                    MessageBox.Show("Phiếu mượn đã tồn tại phiếu bồi thường.");
                else
                {
                    CompensationWindow view = new CompensationWindow();
                    view.ShowDialog();
                }
                    
            });

            //EditIssueBookCommand = new RelayCommand<object>((p) =>
            //{
            //    if (SelectedItem == null)
            //        return false;
            //    return false;
            //}, (p) =>
            //{
            //    CreateIssueWindow view = new CreateIssueWindow();
            //    view.ShowDialog();
            //    //if (!view.IsActive)

            //});

            SeeDetailIssueBookCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                DetailIssueBookWindow view = new DetailIssueBookWindow();
                view.ShowDialog();

            });

            ConfirmReturnBookCommand = new RelayCommand<Grid>((p) =>
            {
                if(SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                var item = (IssueBookList)SelectedItem;
                item.IssueBook.TrangThai = item.IssueBook.TrangThai == 1 ? 0:1;
                DataProvider.Ins.DB.SaveChanges();
                ReturnBookManageViewModel returnBookManageViewModel = (ReturnBookManageViewModel)SelectView;
                returnBookManageViewModel.setDefault();
                CurrentView = SelectView;
                SelectView = null;
                p.Visibility = Visibility.Hidden;
                MessageText = item.IssueBook.TrangThai == 1 ?
                    "Đã huỷ xác nhận trả đối với phiếu mượn có ID" + "\"" + item.IssueBook.ID + "\"."
                    : "Đã xác nhận trả đối với phiếu mượn có ID" + "\"" + item.IssueBook.ID + "\".";
                IsVisible = "Visible";
            });

            ConfirmIndemnifyBookCommand = new RelayCommand<Grid>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                var item = (IndemnifyList)SelectedItem;
                item.IndemnifyBook.TrangThai = item.IndemnifyBook.TrangThai == 1 ? 0 : 1;
                DataProvider.Ins.DB.SaveChanges();
                ReturnBookManageViewModel returnBookManageViewModel = (ReturnBookManageViewModel)SelectView;
                returnBookManageViewModel.setDefault();
                CurrentView = SelectView;
                SelectView = null;
                p.Visibility = Visibility.Hidden;
                MessageText = item.IndemnifyBook.TrangThai == 1 ?
                    "Đã huỷ xác nhận trả đối với phiếu bồi thường có ID" + "\"" + item.IndemnifyBook.ID + "\"."
                    : "Đã xác nhận trả đối với phiếu bồi thường có ID" + "\"" + item.IndemnifyBook.ID + "\".";
                IsVisible = "Visible";
            });

            SeeDetailIndemnifyBookCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                DetailIndemnifyBookWindow view = new DetailIndemnifyBookWindow();
                view.ShowDialog();

            });
        }

        FrameworkElement getParent(Button btn)
        {
            FrameworkElement parent = btn;
            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent;
        }

        private void expandButton(Button btn)
        {
            if(btn == null)
            {
                return;
            }
            else
            {
                Panel panel = (Panel)btn.Content;
                if (panel == null)
                {
                    return;
                }
                else
                {
                    foreach (object child in panel.Children)
                    {
                        if (child is TreeView)
                        {
                            //MessageBox.Show("done");
                            TreeView treeView = (TreeView)child;
                            //MessageBox.Show(treeView.Name);
                            foreach (object item in treeView.Items)
                            {
                                if (item is TreeViewItem)
                                {
                                    //MessageBox.Show(item.ToString());
                                    checkTreeView((TreeViewItem)item);
                                }
                            }
                        }
                    }
                }
                
            }
        }

        private void checkTreeView(TreeViewItem item)
        {
            if (item.Visibility == Visibility.Collapsed)
            {
                item.Visibility = Visibility.Visible;
                item.IsExpanded = true;
            }
            else
            {
                item.Visibility = Visibility.Collapsed;
                item.IsExpanded = false;
            }
        }
    }
}
