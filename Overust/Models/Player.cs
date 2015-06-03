using ITK.Utils;
using ITK.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overust.Models
{
    public class IPAddress: IComparable
    {
        public uint AddressAsDecimal {get; set;}
        public string AddressAsString {get; set;}

        public static IPAddress FromString(string ip)
        {
            return new IPAddress()
            {
                AddressAsDecimal = IPAddressConvert.StringToDecimal(ip),
                AddressAsString = ip,
            };
        }
        public static IPAddress FromDecimal(uint ip)
        {
            return new IPAddress()
            {
                AddressAsDecimal = ip,
                AddressAsString = IPAddressConvert.DecimalToString(ip),
            };
        }

        public override int GetHashCode()
        {
            return AddressAsDecimal.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as IPAddress;
            return other != null && AddressAsDecimal.Equals(other.AddressAsDecimal);
        }

        public override string ToString()
        {
            return AddressAsString;
        }

        public int CompareTo(object obj)
        {
            var other = obj as IPAddress;
            
            if(other != null)
                return AddressAsDecimal.CompareTo(other.AddressAsDecimal);
            else
                return 1;
        }
    }

    public class Player: Notifiable
    {
        private ulong mSteamID;
        private string mCurrentPersonaName;
        private IPAddress mCurrentIPAddress;
        private int mPing;
        private int mCurrentSessionConnectionDuration;
        private bool mIsConnected;
        private bool mIsOnline;

        public ulong SteamID
        {
            get { return mSteamID; }
            set { SetField(ref mSteamID, value); }
        }

        public string CurrentPersonaName
        {
            get { return mCurrentPersonaName; }
            set { SetField(ref mCurrentPersonaName, value); }
        }

        public IPAddress CurrentIPAddress
        {
            get { return mCurrentIPAddress; }
            set { SetField(ref mCurrentIPAddress, value); }
        }

        public int Ping
        {
            get { return mPing; }
            set { SetField(ref mPing, value); }
        }

        public int CurrentSessionConnectionDuration
        {
            get { return mCurrentSessionConnectionDuration; }
            set { SetField(ref mCurrentSessionConnectionDuration, value); }
        }

        public bool IsConnected
        {
            get { return mIsConnected; }
            set { SetField(ref mIsConnected, value); }
        }

        public bool IsOnline
        {
            get { return mIsOnline; }
            set { mIsOnline = value; }
        }

        public HashSet<string> KnownPersonaNames {get; set;}
        public HashSet<IPAddress> KnownIPAddresses {get; set;}

        public bool IsSteamProfilePrivate {get; set;}
        public bool IsSteamProfileConfigured {get; set;}
        public int SteamProfileTimeCreated {get; set;}
        public bool IsFamilyShareAccount {get; set;}
        public ulong FamilyShareLenderSteamID {get; set;}

        public HashSet<ulong> KnownFriends {get; set;}

        public int SteamGameCount {get; set;}
        public int RustPlaytime {get; set;}

        public bool HasVACBan {get; set;}
        public int VACBanCount {get; set;}
        public int DaysSinceLastVACBan {get; set;}

        public bool HasServerBan {get; set;}
        public string ServerBanReason {get; set;}

        public string AvatarSmallURL {get; set;}
        public string AvatarMediumURL {get; set;}
        public string AvatarFullURL {get; set;}
        public string ProfileURL
        {
            get
            {
                return @"http://steamcommunity.com/id/" + SteamID.ToString();
            }
        }


        public Player()
        {
            KnownPersonaNames = new HashSet<string>();
            KnownIPAddresses = new HashSet<IPAddress>();
            KnownFriends = new HashSet<ulong>();
        }

        ////////////////////////////////////////////////////////////////////////////////
        // Steam Data Methods
        ////////////////////////////////////////////////////////////////////////////////
        public void UpdateSteamPlayerData(Steam.Models.Player steamPlayer)
        {
            KnownPersonaNames.Add(CurrentPersonaName);
            CurrentPersonaName = steamPlayer.PersonaName;

            AvatarSmallURL = steamPlayer.AvatarSmallURL;
            AvatarMediumURL = steamPlayer.AvatarMediumURL;
            AvatarFullURL = steamPlayer.AvatarFullURL;

            IsSteamProfilePrivate = steamPlayer.CommunityVisibilityState != Steam.CommunityVisibilityState.Public;
            IsSteamProfileConfigured = steamPlayer.ProfileState == Steam.ProfileState.Setup;
            SteamProfileTimeCreated = steamPlayer.TimeCreated;
        }

        public void UpdateSteamBansData(Steam.Models.PlayerBans bans)
        {
            HasVACBan = bans.IsVACBanned;
            VACBanCount = bans.VACBanCount;
            DaysSinceLastVACBan = bans.DaysSinceLastBan;
        }

        public void UpdateSteamGamesData(Steam.Models.OwnedGames games)
        {
            SteamGameCount = games.GameCount;
        }

        public void UpdateSteamFriendData(Steam.Models.Friend friend)
        {
            KnownFriends.Add(friend.FriendSteamID);
        }

        public void UpdateFamilyShareData(Steam.Models.FamilyShareAccount familyShare)
        {
            IsFamilyShareAccount = familyShare.IsFamilyShareAccount;
            FamilyShareLenderSteamID = familyShare.LenderSteamID;
        }

        ////////////////////////////////////////////////////////////////////////////////
        // Rust Rcon Data Methods
        ////////////////////////////////////////////////////////////////////////////////
        public void UpdateRustPlayerData(Rust.Models.Player player)
        {
            CurrentPersonaName = player.Name;
            KnownPersonaNames.Add(player.Name);
            CurrentIPAddress = IPAddress.FromString(player.IPAddress);
            KnownIPAddresses.Add(CurrentIPAddress);
            CurrentSessionConnectionDuration = player.ConnectedDuration / 60;
            Ping = player.Ping;


            //TODO this might not be needed
            SteamID = player.SteamID;
        }
    }
}
