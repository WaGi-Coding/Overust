using Newtonsoft.Json.Linq;
using Steam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steam.SteamInterface
{
    public class FamilyShareAccountRequest: SteamRequestBase
    {
        public FamilyShareAccountRequest(string apiKey, string steamid, SteamAppID appID):
            base("IPlayerService", "IsPlayingSharedGame", 1, apiKey,
            String.Format("steamid={0}", steamid),
            String.Format("appid_playing={0}", ((int)appID).ToString())){}
    }

    public class FamilyShareAccountResponse: SteamResponseBase<FamilyShareAccount>
    {
        public FamilyShareAccountResponse(SteamHttpResponse steamResponse): base(steamResponse){}
        protected override FamilyShareAccount Parse(string result)
        {
            return JObject.Parse(result)["response"].ToObject<FamilyShareAccount>();
        }
    }
}
