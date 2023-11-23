using MathwolphDndBot.Core;
using MathwolphDndBot.Properties;
using System.Threading;
using System.Threading.Tasks;

namespace MathwolphDndBot.MVVM.ViewModel
{
    internal class ConnexionViewModel : ObservableObject
    {
        private string _chanel = Settings.Default.ChannelName;
        private string _token = Settings.Default.BotToken;

        public GlobalViewModel Global { get; } = GlobalViewModel.Instance;
        public RelayCommand ConnectCommand { get; set; }

        public bool CtrlState
        { 
            get {
                return !Global.Bots.IsConnected;
            }
            set
            {
                OnPropertyChanged();
            }
        }

        public string Chanel
        {
            get { return _chanel; }
            set 
            { 
                _chanel = value;
                OnPropertyChanged();
            }
        }

        public string Token
        {
            get { return _token; }
            set 
            { 
                _token = value; 
                OnPropertyChanged(); 
            }
        }

        public ConnexionViewModel()
        {
            ConnectCommand = new RelayCommand(o => {
                Settings.Default.ChannelName = Chanel;
                Settings.Default.BotToken = Token;

                Settings.Default.Save();

                Global.ChgStateBots();
            });
        }
    }
}
