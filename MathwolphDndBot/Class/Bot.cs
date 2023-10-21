using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;
using System.Diagnostics;

namespace MathwolphDndBot
{
    internal class Bot
    {
        ConnectionCredentials creds = new ConnectionCredentials(TwitchInfo.ChannelName, TwitchInfo.BotToken);
        TwitchClient client;

        public List<User> Players { get; internal set; }
        public List<string> RequestPlayers {  get; internal set; }
        public bool IsConnected { get; internal set; }

        public delegate void OnRequestedPlayerHandler(object sender, string PlayerName);

        public event OnRequestedPlayerHandler OnRequestedPlayer;

        public Bot() 
        {
            IsConnected = false;
        }


        internal void Connect(bool isLogging)
        {
            client = new TwitchClient();
            client.Initialize(creds, TwitchInfo.ChannelName);

            Trace.WriteLine("[Bot]: Connecting...");

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
            Trace.WriteLine("[Bot]: Connected");

            Players = new List<User>();
            RequestPlayers = new List<string>();

            IsConnected = true;
        }

        private void Client_OnChatCommandReceived(object? sender, OnChatCommandReceivedArgs e)
        {
            //Liste des commande pour tout le monde
            switch(e.Command.CommandText.ToLower())
            {
                /*case "dice":
                    client.SendMessage(TwitchInfo.ChannelName, "Ain't got no dice");
                    break;*/
                //case "join":
                //    client.SendMessage(TwitchInfo.ChannelName, "Arrête de le dire et roule le estie qu'on le fume");

                //    break;
                case "joint":
                    if(RequestPlayers.Any(str => str.Contains(e.Command.ChatMessage.DisplayName)))
                    {
                        client.SendMessage(TwitchInfo.ChannelName, "Est le cave tu est déjà en attente d'approbation");

                        break;
                    } 
                    
                    RequestPlayers.Add(e.Command.ChatMessage.DisplayName);

                    OnRequestedPlayer?.Invoke(this, e.Command.ChatMessage.DisplayName);

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
            Trace.WriteLine($"[Bot Error]: {e.Exception.Message}");
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Trace.WriteLine(e.Data);
        }

        internal void Disconnect()
        {
            Trace.WriteLine("[Bot]: Disconnecting ang closing application");

            IsConnected = false;

            client.Disconnect();
        }
    }
}