using Rust.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rust
{
    public enum Opcode
    {
        AuthFailed = -1,
        ServerResponse = 0, // Generic message from server
        Generic,
        Auth,
        Keepalive,
        GetHostname,
        SetHostname,
        GetMaxPlayerCount,
        GetStatus,
        GetPlayers,
        KickPlayer,
        BanPlayer,
    }

    public class Rcon
    {
        public ConnectionInfo CurrentServerInfo {get; set;}
        private Client Client {get; set;}
        public bool IsRunning {get; set;}

        private Dictionary<PacketType, Dictionary<Opcode, Action<Packet>>> PacketHandlers {get; set;}
        //public event EventHandler<ServerResponseEventArgs> ServerResponse;
        public event EventHandler<HostnameEventArgs> HostnameUpdated;
        public event EventHandler<PlayerCountEventArgs> MaxPlayerCountUpdated;
        public event EventHandler<PlayerCountEventArgs> CurrentPlayerCountUpdated;
        public event EventHandler<ConsoleLogEventArgs> ConsoleLogUpdated;
        public event EventHandler<ChatLogEventArgs> ChatLogUpdated;
        public event EventHandler<PlayersEventArgs> PlayersUpdated;
        public event EventHandler<ServerAuthEventArgs> ServerAuthFailed;
        public event EventHandler<ServerAuthEventArgs> ServerAuthSucceeded;
        public event EventHandler<ServerConnectionEventArgs> ServerConnectionFailed;
        public event EventHandler<ServerConnectionEventArgs> ServerConnectionDropped;
        public event EventHandler<ServerConnectionEventArgs> ServerConnectionSucceeded;
        public event EventHandler<ServerConnectionEventArgs> ServerConnectionStarting;
        public event EventHandler<ServerConnectionEventArgs> ServerConnectionDisconnected;

        public Rcon()
        {
            PacketHandlers = new Dictionary<PacketType,Dictionary<Opcode,Action<Packet>>>();
            PacketHandlers[PacketType.Server] = new Dictionary<Opcode,Action<Packet>>();
            PacketHandlers[PacketType.ResponseValue] = new Dictionary<Opcode,Action<Packet>>();
            PacketHandlers[PacketType.AuthResponse] = new Dictionary<Opcode,Action<Packet>>();
            
            PacketHandlers[PacketType.Server][Opcode.ServerResponse]            = OnConsoleLogUpdated;
            PacketHandlers[PacketType.ResponseValue][Opcode.Generic]            = OnConsoleLogUpdated;
            PacketHandlers[PacketType.ResponseValue][Opcode.GetHostname]        = OnGetHostname;
            PacketHandlers[PacketType.ResponseValue][Opcode.GetMaxPlayerCount]  = OnGetMaxPlayerCount;
            PacketHandlers[PacketType.ResponseValue][Opcode.GetStatus]          = OnGetStatus;
            PacketHandlers[PacketType.ResponseValue][Opcode.GetPlayers]         = OnGetPlayers;
            PacketHandlers[PacketType.ResponseValue][Opcode.Keepalive]          = (p) => Console.WriteLine("Keepalive");
            PacketHandlers[PacketType.AuthResponse][Opcode.Auth]                = OnServerAuthSuccess;
            PacketHandlers[PacketType.AuthResponse][Opcode.AuthFailed]          = OnServerAuthFail;
        }

        public bool IsConnected
        {
            get {return Client != null && Client.IsConnected;}
        }
        public async Task Connect(ConnectionInfo connectionInfo)
        {
            // Connect can be called while we're already connected to a server so disconnect first
            Disconnect();

            CurrentServerInfo = connectionInfo;

            Client = new Client();
            Client.ServerConnectionFailed += OnServerConnectionFailed;
            Client.ServerConnectionDropped += OnServerConnectionDropped;
            Client.ServerConnectionSucceeded += OnServerConnectionSucceeded;
            Client.ServerConnectionStarting += OnServerConnectionStarting;
            Client.ServerConnectionDisconnected += OnServerConnectionDisconnected;
            Client.ReceivedPacket += OnReceivedPacket;

            await Client.Connect(CurrentServerInfo.Hostname, CurrentServerInfo.Port);
        }

        public void Disconnect()
        {
            if(IsConnected)
                Client.Disconnect();
        }

        private void PurgeClient()
        {
            CurrentServerInfo = null;
            Client.Dispose();
            Client = null;

            HostnameUpdated(this, new HostnameEventArgs(){NewHostname = "NO SERVER CONNECTION", Timestamp = DateTime.Now});
            MaxPlayerCountUpdated(this, new PlayerCountEventArgs(){PlayerCount = 0});
            CurrentPlayerCountUpdated(this, new PlayerCountEventArgs(){PlayerCount = 0});
        }

        public async Task Run()
        {
            IsRunning = true;
            while(IsRunning)
            {
                if(IsConnected)
                {
                    await Client.Update();
                }
                await Task.Delay(10);
            }
        }

        #region Commands

        public async Task Update()
        {
            await Client.Update();
        }
        public void RequestAuth(string password)
        {
            Client.SendPacket(new Packet(Opcode.Auth, PacketType.Auth, password));
        }

        public bool ExecCommand(Opcode code, string command)
        {
            if(!IsConnected)
                return false;

            Client.SendPacket(new Packet(code, PacketType.ExecCommand, command));
            return true;
        }

        public bool ExecCommand(string command, bool writeToConsole = false)
        {
            if(!ExecCommand(Opcode.Generic, command))
                return false;

            if(writeToConsole && ConsoleLogUpdated != null)
                ConsoleLogUpdated(this, new ConsoleLogEventArgs(){Message = "> " + command, Timestamp = DateTime.Now, IsExecutedCommand = true});

            return true;
        }

        public void Say(string message)
        {
            var formattedMessage = ": " + message;
            if(!ExecCommand("say " + formattedMessage))
                return;

            if(ChatLogUpdated != null)
                ChatLogUpdated(this, new ChatLogEventArgs(){Message = "SERVER CONSOLE" + formattedMessage, Timestamp = DateTime.Now});
        }
        
        public void Say(string message, string nickname)
        {
            var formattedMessage = "(" + nickname + "): " + message;
            if(!ExecCommand("say " + message))
                return;
            
            if(ChatLogUpdated != null)
                ChatLogUpdated(this, new ChatLogEventArgs(){Message = "SERVER CONSOLE" + formattedMessage, Timestamp = DateTime.Now});
        }

        public void Echo(string message)
        {
            ExecCommand("echo " + message);
        }

        public void SetRconPassword(string password)
        {
            ExecCommand(Opcode.Generic, "rcon.password " + password);
        }

        public void SetRconPort(int port)
        {
            ExecCommand(Opcode.Generic, "rcon.port " + port.ToString());
        }

        public void SetRconIp(string address)
        {
            ExecCommand(Opcode.Generic, "rcon.ip " + address);
        }

        public void GetServerHostname()
        {
            ExecCommand(Opcode.GetHostname, "server.hostname");
        }

        public void GetMaxPlayerCount()
        {
            ExecCommand(Opcode.GetMaxPlayerCount, "server.maxplayers");
        }

        public void GetStatus()
        {
            ExecCommand(Opcode.GetStatus, "status");
        }

        public void GetPlayers()
        {
            ExecCommand(Opcode.GetPlayers, "users");
        }

        public void KickPlayer(ulong steamid)
        {
            ExecCommand(Opcode.KickPlayer, "kick " + steamid.ToString());
        }

        public void BanPlayer(ulong steamid)
        {
            ExecCommand(Opcode.BanPlayer, "ban " + steamid.ToString());
        }

        #endregion Commands

        #region Client Handlers

        private void OnReceivedPacket(object sender, PacketEventArgs args)
        {
            var packet = args.Packet;
            //Console.WriteLine(packet.Type.ToString() + "," + packet.Opcode.ToString());
            if(PacketHandlers.ContainsKey(packet.Type))
                if(PacketHandlers[packet.Type].ContainsKey(packet.Opcode))
                    if(PacketHandlers[packet.Type][packet.Opcode] != null)
                        PacketHandlers[packet.Type][packet.Opcode](packet);
            
        }

        private void OnServerConnectionFailed(object sender, ServerConnectionEventArgs args)
        {
            args.ConnectionInfo = CurrentServerInfo;
            PurgeClient();

            if(ServerConnectionFailed != null)
                ServerConnectionFailed(this, args);
        }

        public void OnServerConnectionDropped(object sender, ServerConnectionEventArgs args)
        {
            args.ConnectionInfo = CurrentServerInfo;
            PurgeClient();

            if(ServerConnectionDropped != null)
                ServerConnectionDropped(this, args);
        }

        public void OnServerConnectionSucceeded(object sender, ServerConnectionEventArgs args)
        {
            args.ConnectionInfo = CurrentServerInfo;
            if(ServerConnectionSucceeded != null)
                ServerConnectionSucceeded(this, args);

            RequestAuth(CurrentServerInfo.Password);
        }

        private void OnServerConnectionStarting(object sender, ServerConnectionEventArgs args)
        {
            args.ConnectionInfo = CurrentServerInfo;
            if(ServerConnectionStarting != null)
                ServerConnectionStarting(this, args);
        }

        private void OnServerConnectionDisconnected(object sender, ServerConnectionEventArgs args)
        {
            args.ConnectionInfo = CurrentServerInfo;
            PurgeClient();

            if(ServerConnectionDisconnected != null)
                ServerConnectionDisconnected(this, args);
        }
        #endregion Client Handlers

        #region Rcon Handlers

        private void OnServerAuthSuccess(Packet packet)
        {
            if(ServerAuthSucceeded != null)
                ServerAuthSucceeded(this, new ServerAuthEventArgs{Message = "Successfully authenticated.", Timestamp = packet.Timestamp});

            // This is used to force the disconnect message to send, if there is one, so that the `status` message isn't eaten.
            Echo("Clearing incoming packet stream.");
            GetStatus();
        }

        private void OnServerAuthFail(Packet packet)
        {
            if(ServerAuthFailed != null)
                ServerAuthFailed(this, new ServerAuthEventArgs{Message = "Server authentication failed.  Check that your server password is correct.", Timestamp = packet.Timestamp});

            Disconnect();
        }

        private void OnConsoleLogUpdated(Packet packet)
        {
            var message = packet.DataAsString();

            // TODO: This is an ugly hack to process the chat log. Needs to be fixed on Rust's end.
            if(message.StartsWith("[CHAT] "))
            {
                if(ChatLogUpdated != null)
                    ChatLogUpdated(this, new ChatLogEventArgs(){Message = message.Substring(7, message.Length - 7), Timestamp = packet.Timestamp});

                return;
            }

            if(packet.Opcode == Opcode.Generic || packet.Opcode == Opcode.ServerResponse)// && packet.Type == PacketType.ResponseValue)
            {
                if(ConsoleLogUpdated != null)
                    ConsoleLogUpdated(this, new ConsoleLogEventArgs()
                    {
                        Message = message,
                        Timestamp = packet.Timestamp,
                        IsPublicConsoleMessage = packet.Opcode == Opcode.ServerResponse ? true : false
                    });
            }

        }

        private void OnGetHostname(Packet packet)
        {
            if(HostnameUpdated != null)
            {
                var body = packet.DataAsString();
                HostnameUpdated(this, new HostnameEventArgs(){NewHostname = body.Substring(18, body.Length - 19)});
            }
        }

        private void OnGetMaxPlayerCount(Packet packet)
        {
            if(MaxPlayerCountUpdated != null)
            {
                var body = packet.DataAsString();
                int maxPlayerCount;
                if(int.TryParse(body.Substring(20, body.Length - 21), out maxPlayerCount))
                {
                    MaxPlayerCountUpdated(this, new PlayerCountEventArgs(){PlayerCount = maxPlayerCount});
                }
                else throw new Exception("Failed to parse OnGetMaxPlayerCount");
            }
        }

        private void OnGetPlayers(Packet packet)
        {
            //List<Player> players = new List<Player>();
            //var body = packet.FormattedBody.Substring(
        }

        private void OnGetStatus(Packet packet)
        {
            var str = packet.DataAsString();
            string hostname = "Hostname";
            //string version = "0";
            int maxPlayerCount = 0;
            List<Player> players = new List<Player>();

            int stringIndex = 0;
            for(int iChar = 0, iResult = 0; iChar < str.Length; ++iChar)
            {
                if(str[iChar] == '\n')
                {
                    string data = str.Substring(stringIndex, iChar - stringIndex);
                    stringIndex = iChar + 1;

                    if(data == String.Empty)
                        continue;

                    switch(iResult)
                    {
                        case 0:
                            hostname = ParseHostname(data);
                            break;

                        case 1:
                            //version = ParseVersion(data);
                            break;

                        case 2:
                            //rawMap = data;
                            break;

                        case 3:
                            maxPlayerCount = ParseMaxPlayerCount(data);
                            break;

                        case 4:
                            // Player Column Headers.
                            break;

                        default:
                            players.Add(Player.Parse(data));
                            break;
                    }

                    ++iResult;
                }
            }

            if(HostnameUpdated != null)
            {
                HostnameUpdated(this, new HostnameEventArgs(){NewHostname = hostname});
            }
            if(CurrentPlayerCountUpdated != null)
            {
                CurrentPlayerCountUpdated(this, new PlayerCountEventArgs(){PlayerCount = players.Count});
            }
            if(MaxPlayerCountUpdated != null)
            {
                MaxPlayerCountUpdated(this, new PlayerCountEventArgs(){PlayerCount = maxPlayerCount});
            }
            if(PlayersUpdated != null)
            {
                PlayersUpdated(this, new PlayersEventArgs(){Players = players});
            }
        }



        #endregion Rcon Handlers

        #region Parsers
        // TODO: Move parsers somewhere else

        private static string ParseHostname(string str)
        {
            return str.Substring(10);
        }

        private static string ParseVersion(string str)
        {
            return str;
        }

        private static string ParseMap(string str)
        {
            return str;
        }

        private static int ParseMaxPlayerCount(string str)
        {
            for(int iChar = str.Length - 6; iChar >= 0; --iChar)
            {
                if(str[iChar] == '(')
                {
                    return int.Parse(str.Substring(iChar + 1, str.Length - iChar - 6));
                }
            }

            return 0;
        }

        #endregion Parsers
    }
}
