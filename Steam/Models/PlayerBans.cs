using Newtonsoft.Json;
using ProtoBuf;
using ITK.ModelManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steam.Models
{
    [Model("SteamPlayerBans")]
    [ProtoContract]
    [JsonObject(MemberSerialization.OptIn)]
    public class PlayerBans
    {
        /// <summary>
        /// A string containing the player's 64 bit ID.
        /// </summary>
        [JsonProperty("SteamID")]
        [ProtoMember(1, IsRequired = true)]
        public ulong SteamId {get; set;}

        /// <summary>
        /// Boolean indicating whether or not the player is banned from Community
        /// </summary>
        [JsonProperty("CommunityBanned")]
        [ProtoMember(2)]
        public bool IsCommunityBanned {get; set;}

        /// <summary>
        /// Boolean indicating whether or not the player has VAC bans on record.
        /// </summary>
        [JsonProperty("VACBanned")]
        [ProtoMember(3)]
        public bool IsVACBanned {get; set;}

        /// <summary>
        /// Amount of VAC bans on record.
        /// </summary>
        [JsonProperty("NumberOfVACBans")]
        [ProtoMember(4)]
        public int VACBanCount {get; set;}

        /// <summary>
        /// Amount of days since last ban.
        /// </summary>
        [JsonProperty("DaysSinceLastBan")]
        [ProtoMember(5)]
        public int DaysSinceLastBan {get; set;}

        /// <summary>
        /// String containing the player's ban status in the economy. If the player has no bans on record the string will be "none", if the player is on probation it will say "probation", and so forth.
        /// </summary>
        [JsonProperty("EconomyBan")]
        [ProtoMember(6)]
        public string EconomyBan {get; set;}

        public override int GetHashCode()
        {
            return SteamId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as PlayerBans;
            return other != null && SteamId.Equals(other.SteamId);
        }
    }
}