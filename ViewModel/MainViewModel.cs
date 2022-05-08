using QuanLyThuVien.Model;
using QuanLyThuVien.View;
using System;
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
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand ChangeViewCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
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
                    SelectView = null;
                    CurrentBtn = new Button();
                    expandButton(p);
                    ViewTitle = null;
                    SelectedItem = null;
                    IsClick = false;
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
                switch (ViewTitle)
                {
                    case "Quản lí sách":
                        BookManageViewModel bookManageViewModel = (BookManageViewModel)SelectView;
                        BookList bookList = (BookList)SelectedItem;
                        Sach book = bookList.Book;
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
                        DataProvider.Ins.DB.TheLoais.DeleteObject(bookType);
                        DataProvider.Ins.DB.SaveChanges();
                        bookTypeManageViewModel.setDefault();
                        CurrentView = SelectView;
                        SelectView = null;
                        p.Visibility = Visibility.Hidden;
                        IsVisible = "Visible";
                        break;
                }
                
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
