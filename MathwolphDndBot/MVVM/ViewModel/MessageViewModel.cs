using MathwolphDndBot.Core;
using System;
using System.Windows;

namespace MathwolphDndBot.MVVM.ViewModel
{
    internal class MessageViewModel : ObservableObject
    {
        public GlobalViewModel Global { get; } = GlobalViewModel.Instance;

        public string NbrWaitUsers { get { return Global.Bots.RequestPlayers.Count.ToString(); } }
        public string NbrPlayers { get { return Global.Bots.Players.Count.ToString(); } }
        public string TitlePlayers { get { return $"Joueurs ({Global.Bots.Players.Count.ToString()})"; } }
        public string TitleWaitUsers { get { return $"Utilisateur en attente ({Global.Bots.RequestPlayers.Count.ToString()})"; } }

        public RelayCommand PlayersAcceptedCommand { get; set; }
        public RelayCommand PlayersDeniedCommand { get; set; }
        public RelayCommand ClearPlayerCommand { get; set; }
        public RelayCommand ClearWaitUsersCommand {  get; set; }
        public RelayCommand AcceptRamdomWaitUserCommand { get; set; }

        private int _;

        public int MyProperty
        {
            get { return _; }
            set { _ = value; }
        }


        public MessageViewModel()
        {
            PlayersAcceptedCommand = new RelayCommand(o => {
                Global.Bots.RequestPlayersAccepted(o.ToString());

                this.OnPropertyChanged("TitleWaitUsers");
                this.OnPropertyChanged("TitlePlayers");
                this.OnPropertyChanged("NbrWaitUsers");
                this.OnPropertyChanged("NbrPlayers");
            });

            PlayersDeniedCommand = new RelayCommand(o => {
                Global.Bots.RequestPlayersDenied(o.ToString());

                this.OnPropertyChanged("TitleWaitUsers");
                this.OnPropertyChanged("NbrWaitUsers");
            });

            ClearPlayerCommand = new RelayCommand(o => { 
                Global.Bots.PlayerClear();

                this.OnPropertyChanged("TitlePlayers");
                this.OnPropertyChanged("NbrPlayers");
            });

            ClearWaitUsersCommand = new RelayCommand(o => {
                Global.Bots.RequestPlayersClear();

                this.OnPropertyChanged("TitleWaitUsers");
                this.OnPropertyChanged("NbrWaitUsers");
            });

            AcceptRamdomWaitUserCommand = new RelayCommand(o => {
                if (Global.Bots.RequestPlayers.Count == 0) return;

                Random rnd = new Random();
                int pos = rnd.Next(0, Global.Bots.RequestPlayers.Count);

                Global.Bots.RequestPlayersAccepted(Global.Bots.RequestPlayers[pos]);

                this.OnPropertyChanged("TitleWaitUsers");
                this.OnPropertyChanged("TitlePlayers");
                this.OnPropertyChanged("NbrWaitUsers");
                this.OnPropertyChanged("NbrPlayers");
            });
        }
    }
}
