using MathwolphDndBot.Core;
using System.ComponentModel;

namespace MathwolphDndBot.MVVM.ViewModel
{
    internal class MessageViewModel : ObservableObject
    {
        public GlobalViewModel Global { get; } = GlobalViewModel.Instance;

        public string NbrWaitUsers { get { return $"Utilisateur en attente ({Global.Bots.RequestPlayers.Count.ToString()})"; } }
        public string NbrPlayers { get { return $"Joueurs ({Global.Bots.Players.Count.ToString()})"; } }

        public RelayCommand PlayersAcceptedCommand { get; set; }
        public RelayCommand PlayersDeniedCommand { get; set; }

    public MessageViewModel()
        {
            PlayersAcceptedCommand = new RelayCommand(o => {
                Global.Bots.RequestPlayersAccepted(o.ToString());

                this.OnPropertyChanged("NbrWaitUsers");
                this.OnPropertyChanged("NbrPlayers");
            });

            PlayersDeniedCommand = new RelayCommand(o => {
                Global.Bots.RequestPlayersDenied(o.ToString());

                this.OnPropertyChanged("NbrWaitUsers");
            });
        }
    }
}
