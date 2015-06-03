using Newtonsoft.Json.Linq;
using Steam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steam.SteamInterface
{
    public class PlayerSummariesRequest: SteamRequestBase
    {
        public PlayerSummariesRequest(string apiKey, params long[] steamid):
            this(apiKey, steamid.Select(s => s.ToString()).ToArray()){}

        public PlayerSummariesRequest(string apiKey, params string[] steamid):
            base("ISteamUser", "GetPlayerSummaries", 2, apiKey, string.Format("steamids=[{0}]", string.Join(",", steamid))){}
    }

    public class PlayerSummariesResponse: SteamResponseBase<List<Player>>
    {
        public PlayerSummariesResponse(SteamHttpResponse steamResponse): base(steamResponse){}
        protected override List<Player> Parse(string result)
        {
            return JObject.Parse(result)["response"]["players"].ToObject<List<Player>>();
        }
    }
}
