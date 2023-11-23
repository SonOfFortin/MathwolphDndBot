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
using TwitchLib.Communication.Interfaces;
using System.Threading.Tasks;
using System.Threading;

namespace MathwolphDndBot.Core
{
    internal class Bot : ObservableObject
    {
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
            client.Initialize(new ConnectionCredentials(Settings.Default.ChannelName, Settings.Default.BotToken), Settings.Default.ChannelName);

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
            }

            sendClose = false;
        }

        private void Client_OnDisconnected(object? sender, OnDisconnectedEventArgs e)
        {
            if (IsConnected)
            {
                AddTerminal("[Bot]: Disconnected.", TerminalType.Succes);

                StrConnexionStatus = "Disconnected";

                IsConnected = false;
            }
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
                    var usr = Players.FirstOrDefault(str => str.Name == e.Command.ChatMessage.DisplayName);

                    if (usr != null)
                    {
                        PlayerCommand(e.Command, usr);

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

        private void PlayerCommand(ChatCommand command, User usr)
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

                MessageAdd(usr, msg, DateTime.Now);
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

            return $"Résultat: {String.Join(", ", result)}";
        }

        private void Client_OnMessageReceived(object? sender, OnMessageReceivedArgs e)
        {
            //Trace.WriteLine($"[{e.ChatMessage.DisplayName}]: {e.ChatMessage.Message}");
            var usr = Players.FirstOrDefault(str => str.Name == e.ChatMessage.DisplayName);

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
                    if (((int)(addInfo.Moment - lastMsg.Moment).TotalDays) == 0 && ((int)(DateTime.Now - addInfo.Moment).TotalDays) < 1)
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