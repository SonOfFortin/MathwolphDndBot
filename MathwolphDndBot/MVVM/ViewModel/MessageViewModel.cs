using MathwolphDndBot.Core;
using System;

namespace MathwolphDndBot.MVVM.ViewModel
{
    internal class MessageViewModel : ObservableObject
    {
        public GlobalViewModel Global { get; } = GlobalViewModel.Instance;

        public RelayCommand PlayersAcceptedCommand { get; set; }
        public RelayCommand PlayersDeniedCommand { get; set; }
        public RelayCommand ClearPlayerCommand { get; set; }
        public RelayCommand ClearWaitUsersCommand {  get; set; }
        public RelayCommand AcceptRamdomWaitUserCommand { get; set; }
        public RelayCommand PlayersRemoveCommand { get; set; }

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
            });

            PlayersDeniedCommand = new RelayCommand(o => {
                Global.Bots.RequestPlayersDenied(o.ToString());
            });

            ClearPlayerCommand = new RelayCommand(o => { 
                Global.Bots.PlayerClear();
            });

            ClearWaitUsersCommand = new RelayCommand(o => {
                Global.Bots.RequestPlayersClear();
            });

            AcceptRamdomWaitUserCommand = new RelayCommand(o => {
                if (Global.Bots.RequestPlayers.Count == 0) return;

                Random rnd = new Random();
                int pos = rnd.Next(0, Global.Bots.RequestPlayers.Count);

                Global.Bots.RequestPlayersAccepted(Global.Bots.RequestPlayers[pos]);
            });

            PlayersRemoveCommand = new RelayCommand(o =>
            {
                Global.Bots.RemovePlayers(o.ToString());
            });       
        }
    }
}
