using MaHoa;
using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyThuVien.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private static TaiKhoan _currentAccount;
        public static TaiKhoan CurrentAccount { get { return _currentAccount; } set { _currentAccount = value; } }
        public bool IsLogin { get; set; }
        private string _accountName;
        public string AccountName { get { return _accountName; } set { _accountName = value; OnPropertyChanged(); } }
        private string _password;
        public string Password { get { return _password; } set { _password = value; OnPropertyChanged(); } }
        public ICommand LoginCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                var a = DataProvider.Ins.DB.Saches.ToList();
                Login(p);
            });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => {
                Password = p.Password;
            });
            CloseWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                p.Close();
            });
        }

        void Login(Window p)
        {
            if (p == null)
                return;
            if (AccountName == null || Password == null)
            {
                IsLogin = false;
                MessageBox.Show("Sai tên tài khoản hoặc mật khẩu.");
            }
            else
            {
                string passEncode = Cryption.Vigenere.Encode(Password, AccountName);
                var resultCount = DataProvider.Ins.DB.TaiKhoans.Where(taiKhoan => taiKhoan.TenTaiKhoan == AccountName 
                    && taiKhoan.MatKhau == passEncode).Count();
                if (resultCount > 0)
                {
                    TaiKhoan account = DataProvider.Ins.DB.TaiKhoans.Where(taiKhoan => taiKhoan.TenTaiKhoan == AccountName && taiKhoan.MatKhau == passEncode).SingleOrDefault();
                    if(account.TrangThai == 0)
                    {
                        IsLogin = false;
                        MessageBox.Show("Tài khoản bị khoá.");
                    }
                    else
                    {
                        CurrentAccount = account;
                        IsLogin = true;
                        p.Close();
                    }
                    
                }
                else
                {
                    IsLogin = false;
                    MessageBox.Show("Sai tên tài khoản hoặc mật khẩu.");
                }
            }
        }
    }
}
