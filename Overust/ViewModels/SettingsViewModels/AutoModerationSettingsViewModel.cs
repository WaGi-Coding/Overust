using ITK.WPF;
using Overust.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overust.ViewModels.SettingsViewModels
{
    public class AutoModerationSettingsViewModel: Notifiable
    {
        public AutoModerationSettings AutoModerationSettings {get; set;}

        public AutoModerationSettingsViewModel()
        {
            AutoModerationSettings = App.ModelManager.Get<UserSettings>().AutoModerationSettings;
        }
    }
}
