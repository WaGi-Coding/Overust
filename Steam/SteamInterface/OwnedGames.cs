using Newtonsoft.Json.Linq;
using Steam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steam.SteamInterface
{
    public class OwnedGamesRequest: SteamRequestBase
    {
        /// <param name="steamid">The 64 bit ID of the player.</param>
        /// <param name="detailedInfo">Whether or not to include additional details of apps - name and images. Defaults to false.</param>
        /// <param name="includeFreeGames">Whether or not to list free-to-play games in the results. Defaults to false.</param>
        /// <param name="filter">Restricts results to the appids passed here; does not seem to work as of 31 Mar 2013.</param>
        public OwnedGamesRequest(string apiKey, string steamid, bool detailedInfo = false, bool includeFreeGames = true, params uint[] filter):
            base("IPlayerService", "GetOwnedGames", 1, apiKey,
            String.Format("steamid={0}", steamid),
            String.Format("include_appinfo={0}", detailedInfo ? "1" : "0"),
            String.Format("include_played_free_games={0}", includeFreeGames ? "1" : "0"),
            filter.Length != 0 ? String.Format("appids_filter=[{0}]", string.Join(",", filter)) : null){}
    }

    public class OwnedGamesResponse: SteamResponseBase<OwnedGames>
    {
        public OwnedGamesResponse(SteamHttpResponse steamResponse): base(steamResponse){}
        protected override OwnedGames Parse(string result)
        {
            return JObject.Parse(result)["response"].ToObject<OwnedGames>();
        }
    }
}
