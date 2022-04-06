using QuanLyThuVien.Encryption;
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
                //string passEncode = MD5Hash(Base64Encode(Password));
                //string tmp = MaHoa.Affine.Encode("hentoithubay", 5, 6);
                string passEncode = MaHoa.MD5.Encode(MaHoa.Base64.Encode(Password));
                var resultCount = DataProvider.Ins.DB.TaiKhoans.Where(taiKhoan => taiKhoan.TenTaiKhoan == AccountName && taiKhoan.MatKhau == passEncode).Count();
                if(resultCount > 0)
                {
                    IsLogin = true;
                    p.Close();
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
