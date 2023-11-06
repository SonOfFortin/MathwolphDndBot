using MathwolphDndBot.Core;
using System.Windows;

namespace MathwolphDndBot.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public GlobalViewModel Global { get; } = GlobalViewModel.Instance;

        /* Command */
        public RelayCommand MoveWindowCommand { get; set; }
        public RelayCommand ShutdownWindowCommand { get; set; }
        public RelayCommand MaximizeWindowCommand { get; set; }
        public RelayCommand MinimizeWindowCommand { get; set; }

        public RelayCommand ShowConnexionView {  get; set; }
        public RelayCommand ShowSettingsView { get; set; }
        public RelayCommand ShowConsoleView { get; set; }
        public RelayCommand ShowMessageView { get; set; }
        public RelayCommand ShowUserView {  get; set; }

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

        public ConnexionViewModel ConnexionVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }
        public ConsoleViewModel ConsoleVM { get; set; }
        public MessageViewModel MessageVM { get; set; }
        public UserViewModel UserVM { get; set; }

        public MainViewModel()
        {
            ConnexionVM = new ConnexionViewModel();
            SettingsVM = new SettingsViewModel();
            ConsoleVM = new ConsoleViewModel();
            MessageVM = new MessageViewModel();
            UserVM = new UserViewModel();

            CurrentView = ConnexionVM;

            Application.Current.MainWindow.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            MoveWindowCommand = new RelayCommand(o => { Application.Current.MainWindow.DragMove(); });
            ShutdownWindowCommand = new RelayCommand(o => { Application.Current.Shutdown(); });
            MaximizeWindowCommand = new RelayCommand(o => { Application.Current.MainWindow.WindowState = (Application.Current.MainWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized); });
            MinimizeWindowCommand = new RelayCommand(o => { Application.Current.MainWindow.WindowState = WindowState.Minimized; });

            ShowConnexionView = new RelayCommand(o => { CurrentView = ConnexionVM; });
            ShowSettingsView = new RelayCommand(o => { CurrentView = SettingsVM; });
            ShowConsoleView = new RelayCommand(o => { CurrentView = ConsoleVM; });
            ShowMessageView = new RelayCommand(o => { CurrentView = MessageVM; });
            ShowUserView = new RelayCommand(o => { CurrentView = UserVM; });
        }
    }
}
