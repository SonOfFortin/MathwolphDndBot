using System.Collections.ObjectModel;
using System.Linq;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;
using System.Diagnostics;
using System;
using MathwolphDndBot.MVVM.Model;
using MathwolphDndBot.Properties;
using TwitchLib.Client.Extensions;
using System.Collections.Generic;
using System.Windows.Interop;
using Microsoft.VisualBasic;
using System.Windows;

namespace MathwolphDndBot.Core
{
    internal class Bot : ObservableObject
    {
        private ConnectionCredentials? creds;
        private TwitchClient client = new TwitchClient();
        private Random rnd = new Random();
        private string[] Dices = { "4", "6", "8", "10", "12", "20", "100" };

        private bool sendClose = false;
        private const int _maxElemTerminal = 200;
        private string _strConnexionStatus = string.Empty;
        private bool _isConnected = false;

        public ObservableCollection<User> Players { get; internal set; }
        public ObservableCollection<string> RequestPlayers { get; internal set; }
        public ObservableCollection<Terminal> Terminals { get; internal set; }
        public ObservableCollection<Message> Messages { get; internal set; }

        public bool IsConnected {
            get {  return _isConnected; }
            internal set {  
                _isConnected = value;

                OnPropertyChanged();
            } 
        }

        public string StrConnexionStatus {
            get { return _strConnexionStatus; }
            internal set { 
                _strConnexionStatus = value;

                OnPropertyChanged();
            } 
        }

        public Bot()
        {
            Players = new ObservableCollection<User>();
            RequestPlayers = new ObservableCollection<string>();
            Terminals = new ObservableCollection<Terminal>();
            Messages = new ObservableCollection<Message>();
        }

        internal void Connect()
        {
            if (creds is null)
            {
                creds = new ConnectionCredentials(Settings.Default.ChannelName, Settings.Default.BotToken);
            }

            client.Initialize(creds, Settings.Default.ChannelName);

            StrConnexionStatus = "Connecting...";

            AddTerminal("[Bot]: Connecting...");

            client.OnLog += Client_OnLog;
            client.OnError += Client_OnError;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnChatCommandReceived += Client_OnChatCommandReceived;
            client.OnDisconnected += Client_OnDisconnected;

            client.Connect();
            client.OnConnected += Client_OnConnected;
        }

        private void Client_OnConnected(object? sender, OnConnectedArgs e)
        {
            //On recoir une connexion quand on demande un close WTF?
            //If pour bypasser ce problème rapidement
            if (!sendClose)
            {
                StrConnexionStatus = "Connected";

                AddTerminal("[Bot]: Connected.", TerminalType.Succes);

                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    Players.Clear();
                    RequestPlayers.Clear();
                    Messages.Clear();
                });

                IsConnected = true;

#if DEBUG
                RequestPlayers.Add("cannibal20");
                RequestPlayers.Add("Cannibal21");
                RequestPlayers.Add("Cannibal22");

                var mat = new User
                {
                    Access = Access.Player,
                    Name = "MathWolph"
                };

                var spiky = new User
                {
                    Access = Access.Player,
                    Name = "SpikyPigeon"
                };

                var can = new User
                {
                    Access = Access.Player,
                    Name = "cannibal20"
                };

                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    Players.Add(mat);
                    Players.Add(can);
                    Players.Add(spiky);
                });

                MessageAdd(spiky, "Message 1", new DateTime(2023, 07, 23, 12, 42, 0));
                MessageAdd(spiky, "Message 2", new DateTime(2023, 07, 23, 12, 43, 0));
                MessageAdd(can, "Ben non", DateTime.Now.AddDays(-3).AddMinutes(-6));
                MessageAdd(can, "C'est pas de même", DateTime.Now.AddDays(-3));
                MessageAdd(can, 
                    "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of \"de Finibus Bonorum et Malorum\" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, \"Lorem ipsum dolor sit amet..\", comes from a line in section 1.10.32.", 
                    DateTime.Now);
                MessageAdd(can, "Fait cas autrement", DateTime.Now);
                MessageAdd(mat, "Taisez vous les gas", DateTime.Now);
                MessageAdd(mat, "On test le chat", DateTime.Now);
                MessageAdd(spiky, "Aaaa math", DateTime.Now);
                MessageAdd(spiky, "Salut tlm", DateTime.Now);
                MessageAdd(can, "Lèche botte", DateTime.Now);
                MessageAdd(spiky, "Pk pas", DateTime.Now);
                MessageAdd(mat, 
                    "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.", 
                    DateTime.Now);
#endif
            }

            sendClose = false;
        }

        private void Client_OnDisconnected(object? sender, OnDisconnectedEventArgs e)
        {
            StrConnexionStatus = "Disconnected";

            AddTerminal("[Bot]: Disconnected.", TerminalType.Succes);

            IsConnected = false;
        }

        private void Client_OnChatCommandReceived(object? sender, OnChatCommandReceivedArgs e)
        {
            //Liste des commande pour tout le monde
            switch (e.Command.CommandText.ToLower())
            {
                case "dndjoin":
                    if (RequestPlayers.Any(str => str.Equals(e.Command.ChatMessage.DisplayName)))
                    {
                        client.SendMessage(Settings.Default.ChannelName, $"{e.Command.ChatMessage.DisplayName} tu est déjà en attente d'approbation.");

                        break;
                    }

                    if (Players.Any(str => str.Name.Equals(e.Command.ChatMessage.DisplayName)))
                    {
                        client.SendMessage(Settings.Default.ChannelName, $"{e.Command.ChatMessage.DisplayName} tu est déjà dans les joueurs.");

                        break;
                    }

                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        RequestPlayers.Add(e.Command.ChatMessage.DisplayName);
                    });

                    client.SendMessage(Settings.Default.ChannelName, $"{e.Command.ChatMessage.DisplayName} Votre demande a été envoyé.");

                    break;
                default:
                    if (Players.Any(str => str.Name.Equals(e.Command.ChatMessage.DisplayName)))
                    {
                        PlayerCommand(e.Command);

                        break;
                    }

                    break;
            }

            //Liste des commande de l'Administrateur
            //if (e.Command.ChatMessage.DisplayName.ToLower() == Settings.Default.ChannelName.ToLower())
            //{
            //    switch (e.Command.CommandText.ToLower())
            //    {
            //        case "hi":
            //            client.SendMessage(Settings.Default.ChannelName, "Hi Boss but cannibal20 is your god");
            //            break;
            //    }
            //}
        }

        private void PlayerCommand(ChatCommand command)
        {
            if (command.CommandText.ToLower().Trim() == "roll")
            {
                if (command.ArgumentsAsList.Count != 1)
                {
                    client.SendMessage(Settings.Default.ChannelName, $"{command.ChatMessage.DisplayName} Paramètre non suppoté.");
                    return;
                }

                var msg = RollDice(command.ArgumentsAsList);

                client.SendMessage(Settings.Default.ChannelName, $"{command.ChatMessage.DisplayName} {msg}");
            }
        }

        private string RollDice(List<string> arguments) 
        {
            if (arguments.Count != 1)
            {
                return "Paramètre non suppoté.";
            }


            var tObj = arguments[0].Trim().ToLower().Split('d');

            if (tObj.Length < 1 || tObj.Length > 2)
            {
                return "Paramètre non suppoté.";
            }

            var time = 1;
            var dice = string.Empty;

            if (tObj.Length == 2)
            {
                dice = tObj[1];

                if(tObj[0].Trim() != string.Empty)
                {
                    if (!int.TryParse(tObj[0].Trim(), out time))
                    {
                        return "Nombre de l'ancée non supporté.";
                    }

                    if (time == 0)
                    {
                        return "Nombre de l'ancée non supporté.";
                    }
                }
                
            }
            else
            {
                dice = tObj[0];
            }

            if (!Dices.Contains(dice))
            {
                return "Dé non suppoté.";
            }

            var result = new List<int>();

            for (int i = 0; i < time; i++)
            {
                var s = rnd.Next(1, int.Parse(dice) + 1);

                result.Add(s);
            }

            return $"Résultat:{String.Join(", ", result)}";
        }

        private void Client_OnMessageReceived(object? sender, OnMessageReceivedArgs e)
        {
            //Trace.WriteLine($"[{e.ChatMessage.DisplayName}]: {e.ChatMessage.Message}");
            var usr = Players.First(str => str.Name == e.ChatMessage.DisplayName);

            if (usr != null)
            {
                MessageAdd(usr, e.ChatMessage.Message, DateTime.Now);
            }
        }

        private void MessageAdd(User usr, string message, DateTime moment)
        {
            if (usr != null && message != string.Empty)
            {
                var addInfo = new Message
                {
                    Moment = moment,
                    Msg = message,
                    Type = MessageType.Normal,
                    User = usr
                };

                var lastMsg = Messages.LastOrDefault(new Message
                {
                    User = new User()
                });


                if (lastMsg.User.Name != addInfo.User.Name)
                {
                    addInfo.Type = MessageType.First;
                }
                else
                {
                    if (((int)(addInfo.Moment - lastMsg.Moment).TotalDays) == 0 && ((int)(DateTime.Now - lastMsg.Moment).TotalDays) < 8)
                    {
                        if (((int)(addInfo.Moment - lastMsg.Moment).TotalMinutes) > 5)
                        {
                            addInfo.Type = MessageType.TimeDiff;
                        }
                    }
                    else
                    {
                        addInfo.Type = MessageType.AffDate;
                    }
                }

                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    Messages.Add(addInfo);
                });
            }
        }

        private void Client_OnError(object? sender, OnErrorEventArgs e)
        {
            AddTerminal($"[Bot Error]: {e.Exception.Message}", TerminalType.Error);
        }

        private void Client_OnLog(object? sender, OnLogArgs e)
        {
            AddTerminal(e.Data);
        }

        private void AddTerminal(string msg, TerminalType type = TerminalType.None)
        {
            Trace.WriteLine(msg);

            if (Terminals.Count == _maxElemTerminal)
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    Terminals.Remove(Terminals.First());
                });
            } 

            App.Current.Dispatcher.Invoke((Action)delegate
            {
                Terminals.Add(new Terminal()
                {
                    Message = msg,
                    Moment = DateTime.Now,
                    Type = type
                });
            });
        }

        internal void Disconnect()
        {
            AddTerminal("[Bot]: Disconnecting ang closing application.", TerminalType.Succes);

            StrConnexionStatus = "Disconnecting";

            sendClose = true;

            client.Disconnect();
        }

        public void RequestPlayersAccepted(string name)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                RequestPlayers.Remove(name);

                Players.Add(new User
                {
                    Access = Access.Player,
                    Name = name
                });
            });

            client.SendMessage(Settings.Default.ChannelName, $"{name} Bienvenu dans les joueurs.");
        }

        public void RequestPlayersDenied(string name)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                RequestPlayers.Remove(name);
            });

            client.SendMessage(Settings.Default.ChannelName, $"{name} Votre demande pour joindre les jouers a été refusés!");

        }

        public void RequestPlayersClear()
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                RequestPlayers.Clear();
            });
        }

        public void PlayerClear()
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                Players.Clear();
            });
        }

        public void RemovePlayers(string name) 
        {
            var usr = Players.First(str => str.Name == name);

            if (usr != null)
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    Players.Remove(usr);
                });
            }
        }
    }
}