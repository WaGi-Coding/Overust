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
    [Model("SteamFamilyShareAccounts")]
    [ProtoContract]
    [JsonObject(MemberSerialization.OptIn)]
    public class FamilyShareAccount
    {
        /// <summary>
        /// 64bit SteamID of the user.
        /// </summary>
        [JsonProperty("SteamID")]
        [ProtoMember(1, IsRequired = true)]
        public ulong SteamID {get; set;}

        /// <summary>
        /// 64bit SteamID of the parent family share account.
        /// </summary>
        [JsonProperty("lender_steamid")]
        [ProtoMember(2)]
        public ulong LenderSteamID {get; set;}

        public bool IsFamilyShareAccount
        {
            get
            {
                return SteamID != 0;
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as FamilyShareAccount;
            return other != null && SteamID.Equals(other.SteamID);
        }

        public override int GetHashCode()
        {
            return SteamID.GetHashCode();
        }
    }
}
