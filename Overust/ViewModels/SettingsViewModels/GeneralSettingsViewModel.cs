using ITK.WPF;
using Overust.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overust.ViewModels.SettingsViewModels
{
    public class GeneralSettingsViewModel: Notifiable
    {
        public GeneralSettings GeneralSettings {get; set;}

        public GeneralSettingsViewModel()
        {
            GeneralSettings = App.ModelManager.Get<UserSettings>().GeneralSettings;
        }
    }
}
