using System.Collections.ObjectModel;
using System.Linq;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;
using System.Diagnostics;
using System;
using MathwolphDndBot.MVVM.Model;

namespace MathwolphDndBot.Class
{
    internal class Bot
    {
        ConnectionCredentials? creds;
        TwitchClient client = new TwitchClient();

        bool sendClose = false;

        const int _maxElemTerminal = 200;

        public ObservableCollection<User> Players { get; internal set; }
        public ObservableCollection<string> RequestPlayers {  get; internal set; }
        public ObservableCollection<Terminal> Terminals {  get;  set; }
        public bool IsConnected { get; internal set ; }

        public Bot() 
        {
            IsConnected = false;
            Players = new ObservableCollection<User>();
            RequestPlayers = new ObservableCollection<string>();
            Terminals = new ObservableCollection<Terminal>();

            //Players.Add(new User() { Name = "Cannibal20"});
        }

        internal void Connect(bool isLogging)
        {
            if(creds is null)
            {
                creds = new ConnectionCredentials(TwitchInfo.ChannelName, TwitchInfo.BotToken);
            }

            client.Initialize(creds, TwitchInfo.ChannelName);

            AddTerminal("[Bot]: Connecting...");

            if (isLogging)
                client.OnLog += Client_OnLog;

            client.OnError += Client_OnError;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnChatCommandReceived += Client_OnChatCommandReceived;

            client.Connect();
            client.OnConnected += Client_OnConnected;
        }

        private void Client_OnConnected(object? sender, OnConnectedArgs e)
        {
            //On recoir une connexion quand on demande un close WTF?
            //If pour bypasser ce problème rapidement
            if (!sendClose)
            {
                AddTerminal("[Bot]: Connected", TerminalType.Succes);

                Players.Clear();
                RequestPlayers.Clear();

                IsConnected = true;
            }

            sendClose = false;
        }

        private void Client_OnChatCommandReceived(object? sender, OnChatCommandReceivedArgs e)
        {
            //Liste des commande pour tout le monde
            switch(e.Command.CommandText.ToLower())
            {
                /*case "dice":
                    client.SendMessage(TwitchInfo.ChannelName, "Ain't got no dice");
                    break;*/
                case "join":
                    client.SendMessage(TwitchInfo.ChannelName, "Arrête de le dire et roule le estie qu'on le fume");

                    break;
                case "sexavecmath":
                    client.SendMessage(TwitchInfo.ChannelName, "Math n'est pas ouvert à cette proposition la, il a des morpions. Mais spiky ouiii");
                    break;

                case "sexaveccannibal":
                    client.SendMessage(TwitchInfo.ChannelName, "Ahh, fresh meat!");
                    break;

                case "joint":
                    if(RequestPlayers.Any(str => str.Contains(e.Command.ChatMessage.DisplayName)))
                    {
                        client.SendMessage(TwitchInfo.ChannelName, "Est le cave tu est déjà en attente d'approbation");

                        break;
                    } 
                    
                    RequestPlayers.Add(e.Command.ChatMessage.DisplayName);

                    client.SendMessage(TwitchInfo.ChannelName, "Votre demande a été envoyé");

                    break;
            }

            //Liste des commande de l'Administrateur
            if(e.Command.ChatMessage.DisplayName.ToLower() == TwitchInfo.ChannelName.ToLower())
            {
                switch (e.Command.CommandText.ToLower())
                {
                    case "hi":
                        client.SendMessage(TwitchInfo.ChannelName, "Hi Boss but cannibal20 is your god");
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
            //throw new NotImplementedException();
            AddTerminal($"[Bot Error]: {e.Exception.Message}", TerminalType.Error);
        }

        private void Client_OnLog(object? sender, OnLogArgs e)
        {
            AddTerminal(e.Data);
        }

        private void AddTerminal(string msg, TerminalType type = TerminalType.None)
        {
            Trace.WriteLine(msg);

            //if (Terminals.Count == _maxElemTerminal)
            //    Terminals.Remove(Terminals.First());

            //Terminals.Add(new Terminal(msg, type));
        }

        internal void Disconnect()
        {
            AddTerminal("[Bot]: Disconnecting ang closing application", TerminalType.Succes);

            IsConnected = false;
            sendClose = true;

            client.Disconnect();
        }

        public void AddPlayer(string name)
        {
            RemoveRequestPlayers(name);

            Players.Add(new User() { 
                Name = name,
                Access = Access.Player
            });
        }

        public void RemoveRequestPlayers(string name)
        {
            RequestPlayers.Remove(name);
        }

        public void test() 
        {
            var strName = "Cann" + new Random().Next(0, 99);

            RequestPlayers.Add(strName);
        }
    }
}