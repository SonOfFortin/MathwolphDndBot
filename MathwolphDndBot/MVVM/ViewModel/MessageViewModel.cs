using MathwolphDndBot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MathwolphDndBot.MVVM.ViewModel
{
    internal class MessageViewModel : ObservableObject
    {
        public GlobalViewModel Global { get; } = GlobalViewModel.Instance;

        public string NbrWaitUsers { get { return $"Utilisateur en attente ({Global.Bots.RequestPlayers.Count.ToString()})"; } }

        public RelayCommand AddUsersCommand { get; set; }

        public MessageViewModel()
        {
            AddUsersCommand = new RelayCommand(o => {
                Console.WriteLine("ClickMode AddUsersCommand");

                Global.Bots.addTestUser();
            });

        }
    }
}
