using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyThuVien.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private static bool _isEditClick;
        public static bool IsEditClick { get { return _isEditClick; } set { _isEditClick = value; } }
        private bool _isEnable;
        public bool IsEnable { get { return _isEnable; } set { _isEnable = value; OnPropertyChanged(); } }
        private static object? _selectedItem;
        public static object? SelectedItem { get { return _selectedItem; } set { _selectedItem = value; } }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void setIsEnable()
        {
            IsEnable = true? IsEnable = false : IsEnable = true;
        }
    }

    class RelayCommand<T> : ICommand
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        public RelayCommand(Predicate<T> canExecute, Action<T> execute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _canExecute = canExecute;
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
