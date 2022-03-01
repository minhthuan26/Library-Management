using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyThuVien.ViewModel
{
    public class ControlBarViewModel : BaseViewModel
    {
        #region commands
        public ICommand CloseWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set; }
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MouseMoveWindowCommand { get; set; }
        #endregion

        public ControlBarViewModel()
        {
            CloseWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => {
                FrameworkElement window = getWindowParent(p);
                var windowCheck = window as Window;
                if (windowCheck != null)
                {
                    windowCheck.Close();
                }
            });

            MaximizeWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => {
                FrameworkElement window = getWindowParent(p);
                var windowCheck = window as Window;
                if (windowCheck != null)
                {
                    if (windowCheck.WindowState != WindowState.Maximized)
                        windowCheck.WindowState = WindowState.Maximized;
                    else
                        windowCheck.WindowState = WindowState.Normal;
                }
            });

            MinimizeWindowCommand = new RelayCommand<UserControl>((p) => { return p==null? false:true; }, (p) => {
                FrameworkElement window = getWindowParent(p);
                var windowCheck = window as Window;
                if (windowCheck != null)
                {
                    if (windowCheck.WindowState != WindowState.Minimized)
                        windowCheck.WindowState = WindowState.Minimized;
                    else
                        windowCheck.WindowState = WindowState.Maximized;
                }
            });

            MouseMoveWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => {
                FrameworkElement window = getWindowParent(p);
                var windowCheck = window as Window;
                if (windowCheck != null)
                {
                    windowCheck.DragMove();
                }
            });
        }

        FrameworkElement getWindowParent(UserControl uc)
        {
            FrameworkElement parent = uc;
            while(parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent;
        }
    }
}
