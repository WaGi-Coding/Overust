using Overust.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overust.ViewModels.SettingsViewModels
{
    public class ConsoleSettingsViewModel
    {
        public ConsoleSettings ConsoleSettings {get; set;}

        public ConsoleSettingsViewModel()
        {
            ConsoleSettings = App.ModelManager.Get<UserSettings>().ConsoleSettings;
        }
    }
}
