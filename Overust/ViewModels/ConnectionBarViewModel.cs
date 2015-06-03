using ITK.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overust.ViewModels
{
    public class ConnectionBarViewModel: Notifiable
    {
        private string mConnectionStatus;
        public string ConnectionStatus
        {
            get {return mConnectionStatus;}
            set {SetField(ref mConnectionStatus, value);}
        }

        private string mServerDisplayName;
        public string ServerDisplayName
        {
            get {return mServerDisplayName;}
            set {SetField(ref mServerDisplayName, value);}
        }

        private string mCurrentPlayerCount;
        public string CurrentPlayerCount
        {
            get {return mCurrentPlayerCount;}
            set {SetField(ref mCurrentPlayerCount, value);}
        }

        private string mMaxPlayerCount;
        public string MaxPlayerCount
        {
            get {return mMaxPlayerCount;}
            set {SetField(ref mMaxPlayerCount, value);}
        }

        public ConnectionBarViewModel()
        {
            ConnectionStatus = Rust.ServerConnectionStatus.Disconnected.ToString();
            ServerDisplayName = "NO SERVER CONNECTION";
            CurrentPlayerCount = "0";
            MaxPlayerCount = "0";

            App.RustRcon.ServerConnectionStarting +=        (s, args) => ConnectionStatus = args.Status.ToString();
            App.RustRcon.ServerConnectionSucceeded +=       (s, args) => ConnectionStatus = args.Status.ToString();
            App.RustRcon.ServerConnectionFailed +=          (s, args) => ConnectionStatus = args.Status.ToString();
            App.RustRcon.ServerConnectionDisconnected +=    (s, args) => ConnectionStatus = args.Status.ToString();
            App.RustRcon.ServerConnectionDropped +=         (s, args) => ConnectionStatus = args.Status.ToString();
            App.RustRcon.HostnameUpdated +=                 (s, args) => ServerDisplayName = args.NewHostname;
            App.RustRcon.CurrentPlayerCountUpdated +=       (s, args) => CurrentPlayerCount = args.PlayerCount.ToString();
            App.RustRcon.MaxPlayerCountUpdated +=           (s, args) => MaxPlayerCount = args.PlayerCount.ToString();
        }
    }
}
