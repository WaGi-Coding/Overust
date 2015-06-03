using Overust.Models;
using Rust;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overust
{
    public class LogManager
    {
        private List<ChatLogEventArgs> ChatLog {get; set;}
        private List<ConsoleLogEventArgs> ConsoleLog {get; set;}

        public LogManager()
        {
            ChatLog = new List<ChatLogEventArgs>(250);
            ConsoleLog = new List<ConsoleLogEventArgs>(250);

            App.RustRcon.ChatLogUpdated += (s, args) =>
                {
                    ChatLog.Add(args);
                };

            App.RustRcon.ConsoleLogUpdated += (s, args) =>
                {
                    ConsoleLog.Add(args);
                };
        }

        public void SaveLog()
        {
            if(!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");

            var chatSettings = App.ModelManager.Get<UserSettings>().ChatSettings;
            var consoleSettings = App.ModelManager.Get<UserSettings>().ConsoleSettings;

            string directory = "Overust Log " + DateTime.Now.ToString("MM-dd-yy");

            if(chatSettings.IsLoggingEnabled || consoleSettings.IsLoggingEnabled)
                if(!Directory.Exists(@"Logs\" + directory))
                    Directory.CreateDirectory(@"Logs\" + directory);

            if(chatSettings.IsLoggingEnabled)
            {
                string chat = "";

                foreach(var entry in ChatLog)
                {
                    if(chatSettings.IsLogTimestampingEnabled)
                        chat += entry.Timestamp.ToString("(hh:mm tt) ");

                    chat += entry.Message + Environment.NewLine;
                }

                File.AppendAllText(@"Logs\" + directory + @"\Chat Log " + DateTime.Now.ToString("MM-dd-yy") + ".txt", chat);
            }

            if(consoleSettings.IsLoggingEnabled)
            {
                string console = "";

                foreach(var entry in ConsoleLog)
                {
                    if(entry.IsExecutedCommand ||
                        (entry.IsPublicConsoleMessage && consoleSettings.IsPublicConsoleLoggingEnabled) ||
                        (!entry.IsPublicConsoleMessage && !consoleSettings.IsPublicConsoleLoggingEnabled))
                    {
                        if(consoleSettings.IsLogTimestampingEnabled)
                            console += entry.Timestamp.ToString("(hh:mm tt) ");

                        console += entry.Message + Environment.NewLine;
                    }
                }

                File.AppendAllText(@"Logs\" + directory + @"\Console Log " + DateTime.Now.ToString("MM-dd-yy") + ".txt", console);
            }
        }
    }
}
