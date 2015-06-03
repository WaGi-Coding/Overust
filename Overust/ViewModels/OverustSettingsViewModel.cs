using ITK.WPF;
using Overust.Views.SettingsViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Overust.ViewModels
{
    public class SettingsSection
    {
        public string Title {get; set;}
        public UserControl Content {get; set;}
    }
    public class OverustSettingsViewModel: Notifiable
    {
        public ObservableCollection<SettingsSection> Sections {get; set;}

        public OverustSettingsViewModel()
        {
            Sections = new ObservableCollection<SettingsSection>();

            Sections.Add(new SettingsSection(){Title = "General", Content = new GeneralSettingsView()});
            Sections.Add(new SettingsSection(){Title = "Chat", Content = new ChatSettingsView()});
            Sections.Add(new SettingsSection(){Title = "Console", Content = new ConsoleSettingsView()});
            //Sections.Add(new SettingsSection(){Title = "Auto Moderation", Content = new AutoModerationSettingsView()});
        }
    }
}
