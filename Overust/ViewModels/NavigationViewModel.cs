using ITK.WPF;
using Overust.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Overust.ViewModels
{
    public class NavigationViewModel: Notifiable
    {
        public ObservableCollection<NavigationItem> NavigationItems {get; set;}

        public NavigationViewModel()
        {
            NavigationItems = new ObservableCollection<NavigationItem>();

            AddNavigationItem("Home", new HomeView());
            AddNavigationItem("Players", new PlayersView());
            AddNavigationItem("Chat", new ChatView());
            AddNavigationItem("Console", new ConsoleView());
            AddNavigationItem("Overust Settings", new OverustSettingsView());
        }

        private void AddNavigationItem(string header, UserControl content)
        {
            NavigationItems.Add(new NavigationItem(){Header = header, Content = content});
        }
    }

    public class NavigationItem
    {
        public string Header {get; set;}
        public UserControl Content {get; set;}
    }
}
