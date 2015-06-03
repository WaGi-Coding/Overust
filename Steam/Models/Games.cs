using Newtonsoft.Json;
using ProtoBuf;
using ITK.ModelManager;
using ITK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steam.Models
{
    [Model("SteamPlayerGames")]
    [ProtoContract]
    [JsonObject(MemberSerialization.OptIn)]
    public class Game
    {
        /// <summary>
        /// Owner's 64-bit SteamID
        /// </summary>
        [ProtoMember(1, IsRequired = true)]
        public ulong OwnerSteamID {get; set;}

        /// <summary>
        /// AppID of the game.
        /// </summary>
        [JsonProperty("appid")]
        [ProtoMember(2)]
        public uint AppID {get; set;}

        /// <summary>
        /// The name of the game.
        /// </summary>
        [JsonProperty("name")]
        [ProtoMember(3)]
        public string Name {get; set;}

        /// <summary>
        /// Total playtime.
        /// </summary>
        [JsonProperty("playtime_forever")]
        [ProtoMember(4)]
        public int PlaytimeForever {get; set;}

        /// <summary>
        /// Playtime over the past 2 weeks.
        /// </summary>
        [JsonProperty("playtime_2weeks")]
        [ProtoMember(5)]
        public int PlaytimeRecent {get; set;}

        /// <summary>
        /// The hash part for the icon url.
        /// </summary>
        [JsonProperty("img_icon_url")]
        [ProtoMember(6)]
        public string IconHash {get; set;}

        /// <summary>
        /// The hash part for the logo url.
        /// </summary>
        [JsonProperty("img_logo_url")]
        [ProtoMember(7)]
        public string LogoHash {get; set;}

        public string IconURL
        {
            get {return String.Format("http://media.steampowered.com/steamcommunity/public/images/apps/{0}/{1}.jpg", AppID.ToString(), IconHash);}
        }

        public string LogoURL
        {
            get {return String.Format("http://media.steampowered.com/steamcommunity/public/images/apps/{0}/{1}.jpg", AppID.ToString(), LogoHash);}
        }

        public override bool Equals(object obj)
        {
            var other = obj as Game;
            return other != null && OwnerSteamID.Equals(other.OwnerSteamID) && AppID.Equals(other.AppID);
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder(OwnerSteamID).Add(AppID).GetHashCode();
        }
    }
    
    [JsonObject(MemberSerialization.OptIn)]
    public class OwnedGames
    {
        [JsonProperty("game_count")]
        public int GameCount {get; set;}

        [JsonProperty("games")]
        public List<Game> Games {get; set;}
    }
}
