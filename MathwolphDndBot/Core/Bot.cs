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

namespace MathwolphDndBot.Core
{
    internal class Bot : ObservableObject
    {
        private ConnectionCredentials? creds;
        private TwitchClient client = new TwitchClient();

        private bool sendClose = false;
        private const int _maxElemTerminal = 200;
        private string _strConnexionStatus = string.Empty;
        private bool _isConnected = false;

        public ObservableCollection<User> Players { get; internal set; }
        public ObservableCollection<string> RequestPlayers { get; set; }
        public ObservableCollection<Terminal> Terminals { get; set; }

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

                AddTerminal("[Bot]: Connected", TerminalType.Succes);

                Players.Clear();
                RequestPlayers.Clear();

                IsConnected = true;

                addTestUser();
            }

            sendClose = false;
        }

        public void addTestUser()
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                RequestPlayers.Add("Cannibal20");
                RequestPlayers.Add("Cannibal21");
                RequestPlayers.Add("Cannibal22");
            });
        }

        private void Client_OnDisconnected(object? sender, OnDisconnectedEventArgs e)
        {
            StrConnexionStatus = "Disconnected";

            AddTerminal("[Bot]: Disconnected", TerminalType.Succes);

            IsConnected = false;
        }

        private void Client_OnChatCommandReceived(object? sender, OnChatCommandReceivedArgs e)
        {
            //Liste des commande pour tout le monde
            switch (e.Command.CommandText.ToLower())
            {
                /*case "dice":
                    client.SendMessage(Settings.Default.ChannelName, "Ain't got no dice");
                    break;*/
                case "join":
                    client.SendMessage(Settings.Default.ChannelName, "Arrête de le dire et roule le estie qu'on le fume");

                    break;
                case "sexavecmath":
                    client.SendMessage(Settings.Default.ChannelName, "Math n'est pas ouvert à cette proposition la, il a des morpions. Mais spiky ouiii");
                    break;

                case "sexaveccannibal":
                    client.SendMessage(Settings.Default.ChannelName, "Ahh, fresh meat!");
                    break;

                case "joint":
                    if (RequestPlayers.Any(str => str.Contains(e.Command.ChatMessage.DisplayName)))
                    {
                        client.SendMessage(Settings.Default.ChannelName, "Est le cave tu est déjà en attente d'approbation");

                        break;
                    }

                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        RequestPlayers.Add(e.Command.ChatMessage.DisplayName);
                    });

                    client.SendMessage(Settings.Default.ChannelName, "Votre demande a été envoyé");

                    break;
            }

            //Liste des commande de l'Administrateur
            if (e.Command.ChatMessage.DisplayName.ToLower() == Settings.Default.ChannelName.ToLower())
            {
                switch (e.Command.CommandText.ToLower())
                {
                    case "hi":
                        client.SendMessage(Settings.Default.ChannelName, "Hi Boss but cannibal20 is your god");
                        break;
                }
            }
        }

        private void Client_OnMessageReceived(object? sender, OnMessageReceivedArgs e)
        {
            Trace.WriteLine($"[{e.ChatMessage.DisplayName}]: {e.ChatMessage.Message}");
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
            AddTerminal("[Bot]: Disconnecting ang closing application", TerminalType.Succes);

            StrConnexionStatus = "Disconnecting";

            sendClose = true;

            client.Disconnect();
        }
    }
}