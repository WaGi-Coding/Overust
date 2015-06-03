using Overust.Models;
using Overust.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Overust.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeViewModel ViewModel {get; set;}

        public HomeView()
        {
            ViewModel = new HomeViewModel();
            ViewModel.ApplicationLogUpdated += (s, args) => ApplicationLogScrollViewer.ScrollToBottom();
            InitializeComponent();
        }

        private void NewServerButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.ServerModelSet.NewServer();
            ServersListView.SelectedIndex = ServersListView.Items.Count - 1;
        }

        private void DeleteServerButtonClick(object sender, RoutedEventArgs e)
        {
            DeleteSelectedServer();
        }

        private void ServersListView_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete && ServersListView.HasItems && ServersListView.SelectedItem != null)
            {
                DeleteSelectedServer();
            }
        }

        private void DeleteSelectedServer()
        {
            var selectedServer = ServersListView.SelectedItem as Server;
            if(selectedServer == null || selectedServer.IsConnected)
                return;

            int currentIndex = ServersListView.SelectedIndex;
            ViewModel.ServerModelSet.DeleteServer(selectedServer);

            if(ServersListView.Items.Count != 0)
                ServersListView.SelectedIndex = currentIndex < ServersListView.Items.Count ? currentIndex : ServersListView.Items.Count - 1;
        }

        private async void ConnectButtonClick(object sender, RoutedEventArgs e)
        {
            var server = ServersListView.SelectedItem as Server;
            if(server != null)
            {
                ViewModel.AttemptReconnectOnServerConnectionFail = false;
                await App.RustRcon.Connect(server);
            }
        }

        private async void ServerListViewItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var server = ServersListView.SelectedItem as Server;
            if(server != null)
            {
                ViewModel.AttemptReconnectOnServerConnectionFail = false;
                await App.RustRcon.Connect(server);
            }
        }

        private void DisconnectButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.AttemptReconnectOnServerConnectionFail = false;
            App.RustRcon.Disconnect();
        }
    }
}
