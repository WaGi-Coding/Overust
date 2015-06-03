using ITK.WPF;
using Overust.Models;
using Rust;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overust.ViewModels
{
    public class ConsoleViewModel: Notifiable
    {
        public ConsoleSettings ConsoleSettings {get; set;}

        private string mConsole;
        public string Console
        {
            get {return mConsole;}
            set {SetField(ref mConsole, value);}
        }

        public ConsoleViewModel()
        {
            ConsoleSettings = App.ModelManager.Get<UserSettings>().ConsoleSettings;

            App.RustRcon.ConsoleLogUpdated += (s, args) =>
                {
                    if(args.IsExecutedCommand ||
                        (args.IsPublicConsoleMessage && ConsoleSettings.ShowPublicConsoleLog) ||
                        (!args.IsPublicConsoleMessage && !ConsoleSettings.ShowPublicConsoleLog))
                        WriteToConsole(args.Message, args.Timestamp);
                };
        }

        private void WriteToConsole(string message, DateTime timestamp)
        {
            if(ConsoleSettings.IsTimestampingEnabled)
                Console += timestamp.ToString("(hh:mm tt) ") + message + System.Environment.NewLine;
            else
                Console += message + System.Environment.NewLine;
        }
    }
}
