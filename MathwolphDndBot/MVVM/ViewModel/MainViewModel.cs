using MathwolphDndBot.Core;
using System.Windows;

namespace MathwolphDndBot.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        /* Command */
        public RelayCommand MoveWindowCommand { get; set; }
        public RelayCommand ShutdownWindowCommand { get; set; }
        public RelayCommand MaximizeWindowCommand { get; set; }
        public RelayCommand MinimizeWindowCommand { get; set; }

        public RelayCommand ShowProtectionView {  get; set; }
        public RelayCommand ShowSettingsView { get; set; }
        public RelayCommand ShowConsoleView { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public ProtectionViewModel ProtectionVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }
        public ConsoleViewModel ConsoleVM { get; set; }

        public MainViewModel()
        {
            ProtectionVM = new ProtectionViewModel();
            SettingsVM = new SettingsViewModel();
            ConsoleVM = new ConsoleViewModel();

            CurrentView = ProtectionVM;

            Application.Current.MainWindow.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            MoveWindowCommand = new RelayCommand(o => { Application.Current.MainWindow.DragMove(); });
            ShutdownWindowCommand = new RelayCommand(o => { Application.Current.Shutdown(); });
            MaximizeWindowCommand = new RelayCommand(o => { Application.Current.MainWindow.WindowState = (Application.Current.MainWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized); });
            MinimizeWindowCommand = new RelayCommand(o => { Application.Current.MainWindow.WindowState = WindowState.Minimized; });

            ShowProtectionView = new RelayCommand(o => { CurrentView = ProtectionVM; });
            ShowSettingsView = new RelayCommand(o => { CurrentView = SettingsVM; });
            ShowConsoleView = new RelayCommand(o => { CurrentView =  ConsoleVM; });
        }
    }
}
