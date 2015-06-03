using ITK.WPF;
using Overust.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overust.ViewModels
{
    public class HomeViewModel: Notifiable
    {
        public ServerModelSet ServerModelSet {get; set;}
        public bool AttemptReconnectOnServerConnectionFail {get; set;}
        public event EventHandler ApplicationLogUpdated;

        public HomeViewModel()
        {
            ServerModelSet = App.ModelManager.Get<ServerModelSet>();

            //App.RustRcon.ServerConnectionDisconnected += (s, args) => ServerModelSet.SetConnectedServer(null);
            //App.RustRcon.ServerConnectionDropped += (s, args) => ServerModelSet.SetConnectedServer(null);
            //App.RustRcon.ServerConnectionFailed += (s, args) => ServerModelSet.SetConnectedServer(null);
            
            ApplicationLog = "";
            App.RustRcon.ServerConnectionStarting += (s, args) =>
                {
                    ((Server)args.ConnectionInfo).IsConnected = true;
                    WriteToApplicationLog(args.Message, args.Timestamp);
                };
            App.RustRcon.ServerConnectionSucceeded += (s, args) => WriteToApplicationLog(args.Message, args.Timestamp);
            App.RustRcon.ServerConnectionDisconnected += (s, args) =>
                {
                    ((Server)args.ConnectionInfo).IsConnected = false;
                    WriteToApplicationLog(args.Message, args.Timestamp);
                };
            App.RustRcon.ServerConnectionFailed += (s, args) =>
                {
                    ((Server)args.ConnectionInfo).IsConnected = false;
                    WriteToApplicationLog(args.Message, args.Timestamp);

                    if(AttemptReconnectOnServerConnectionFail && App.ModelManager.Get<UserSettings>().GeneralSettings.IsAutoReconnectEnabled)
                        InitiateReconnect(args.ConnectionInfo as Server);
                };
            App.RustRcon.ServerConnectionDropped += (s, args) =>
                {
                    ((Server)args.ConnectionInfo).IsConnected = false;
                    WriteToApplicationLog(args.Message, args.Timestamp);

                    if(App.ModelManager.Get<UserSettings>().GeneralSettings.IsAutoReconnectEnabled)
                    {
                        InitiateReconnect(args.ConnectionInfo as Server);
                        AttemptReconnectOnServerConnectionFail = true;
                    }
                };
            App.RustRcon.ServerAuthSucceeded += (s, args) => WriteToApplicationLog(args.Message, args.Timestamp);
            App.RustRcon.ServerAuthFailed += (s, args) => WriteToApplicationLog(args.Message, args.Timestamp);
        }

        private string mApplicationLog;
        public string ApplicationLog
        {
            get { return mApplicationLog; }
            set { SetField(ref mApplicationLog, value); }
        }

        private void WriteToApplicationLog(string message, DateTime timestamp)
        {
            //if(ConsoleSettings.IsTimestampingEnabled)
            //    Console += timestamp.ToString("(hh:mm tt) ") + message + System.Environment.NewLine;
            //else
            //    Console += message + System.Environment.NewLine;

            ApplicationLog += System.Environment.NewLine + timestamp.ToString("(hh:mm tt) ") + message;
            if(ApplicationLogUpdated != null)
                ApplicationLogUpdated(this, null);

        }

        private async void Run()
        {

        }

        private void InitiateReconnect(Server server)
        {
            WriteToApplicationLog("Auto-Reconnecting in 10 seconds...", DateTime.Now);
            App.EventManager.Events.Add(new TimedEvent(10000, () =>
                {
                    if(!App.RustRcon.IsConnected)
                    {
                        App.RustRcon.Connect(server);
                    }
                }, true));
        }
    }
}
