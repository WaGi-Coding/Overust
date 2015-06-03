using Overust.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public enum PlayerCategoryType
    {
        All,
        Online,
        Offline,
        Banned,
    }

    public class PlayerCategory
    {
        public string Title {get; set;}
        public ObservableCollection<Player> Players {get; set;}

        public PlayerCategory()
        {
            Players = new ObservableCollection<Player>();
        }
    }

    /// <summary>
    /// Interaction logic for PlayersView.xaml
    /// </summary>
    public partial class PlayersView : UserControl
    {
        public Dictionary<ulong, Player> Players {get; set;}
        public Dictionary<PlayerCategoryType, PlayerCategory> PlayerCategories {get; set;}

        // Categories.Add({Title = "Online", p => p.Online});
        // Categories.Add
        
        public PlayersView()
        {
            Players = new Dictionary<ulong, Player>();
            PlayerCategories = new Dictionary<PlayerCategoryType, PlayerCategory>();
            PlayerCategories.Add(PlayerCategoryType.All, new PlayerCategory(){Title = "All"});
            PlayerCategories.Add(PlayerCategoryType.Online, new PlayerCategory(){Title = "Online"});
            PlayerCategories.Add(PlayerCategoryType.Offline, new PlayerCategory(){Title = "Offline"});
            PlayerCategories.Add(PlayerCategoryType.Banned, new PlayerCategory(){Title = "Banned"});

            App.RustRcon.PlayersUpdated += (s, args) =>
                {
                    var onlinePlayers = PlayerCategories[PlayerCategoryType.Online].Players;
                    var allPlayers = PlayerCategories[PlayerCategoryType.All].Players;

                    //if(PlayerCategoriesListView.SelectedItem

                    foreach(var player in onlinePlayers)
                    {
                        player.IsOnline = false;
                    }

                    foreach(var rustPlayerData in args.Players)
                    {
                        Player player;

                        if(!Players.ContainsKey(rustPlayerData.SteamID))
                        {
                            player = new Player();
                            player.UpdateRustPlayerData(rustPlayerData);
                            Players.Add(player.SteamID, player);
                            allPlayers.Add(player);
                        }
                        else
                        {
                            player = Players[rustPlayerData.SteamID];
                            player.UpdateRustPlayerData(rustPlayerData);
                        }

                        player.IsOnline = true;

                        if(!onlinePlayers.Contains(player))
                            onlinePlayers.Add(player);
                    }

                    for(int iPlayer = 0; iPlayer < onlinePlayers.Count; ++iPlayer)
                    {
                        if(!onlinePlayers[iPlayer].IsOnline)
                        {
                            onlinePlayers.RemoveAt(iPlayer);
                            --iPlayer;
                        }
                    }
                };

            //for(int i = 0; i < 50; ++i)
            //{
            //    Players.Add(new Player()
            //{
            //    SteamID = 76561198142422120,
            //    CurrentPersonaName = "ooooooooooooooooooooooooooooooo" + i.ToString(),
            //    KnownPersonaNames = new HashSet<string>(){
            //        "inlinevoid",
            //        "LikeSubscribeDerp",
            //        "Codestream",
            //        "Jookie",
            //        "FaggotBurger",
            //        "RustPlayer"},
            //    Ping = 2000 + i,
            //    CurrentSessionConnectionDuration = 34 + i,
            //    IsConnected = true,
            //    IsSteamProfilePrivate = false,
            //    IsSteamProfileConfigured = true,
            //    IsFamilyShareAccount = true,
            //    FamilyShareLenderSteamID = 76561198142422120,
            //    VACBanCount = 1,
            //    CurrentIPAddress = IPAddress.FromString("127.127.127.1" + i.ToString())
            //});
            //}
            InitializeComponent();
        }

        private void OpenSteamProfileButtonClick(object sender, RoutedEventArgs e)
        {
            var player = PlayersListView.SelectedItem as Player;
            Process.Start("http://steamcommunity.com/profiles/" + player.SteamID);
        }

        private void CopyPlayerSteamIDButtonClick(object sender, RoutedEventArgs e)
        {
            var player = PlayersListView.SelectedItem as Player;
            Clipboard.SetText(player.SteamID.ToString());
        }

        private void CopyPlayerNameButtonClick(object sender, RoutedEventArgs e)
        {
            var player = PlayersListView.SelectedItem as Player;
            Clipboard.SetText(player.CurrentPersonaName);
        }

        private void CopyPlayerIPButtonClick(object sender, RoutedEventArgs e)
        {
            var player = PlayersListView.SelectedItem as Player;
            Clipboard.SetText(player.CurrentIPAddress.ToString());
        }

        private void KickButtonClick(object sender, RoutedEventArgs e)
        {
            var player = PlayersListView.SelectedItem as Player;
            App.RustRcon.KickPlayer(player.SteamID);
        }

        private void BanButtonClick(object sender, RoutedEventArgs e)
        {
            var player = PlayersListView.SelectedItem as Player;
            App.RustRcon.BanPlayer(player.SteamID);
        }
    }
}
