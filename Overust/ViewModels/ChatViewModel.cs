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
    public class ChatViewModel: Notifiable
    {
        public ChatSettings ChatSettings {get; set;}
        public AutoModerationSettings AutoModerationSettings {get; set;}

        public ChatViewModel()
        {
            ChatSettings = App.ModelManager.Get<UserSettings>().ChatSettings;
            AutoModerationSettings = App.ModelManager.Get<UserSettings>().AutoModerationSettings;
        }
    }
}