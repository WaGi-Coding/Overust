using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Steam.Models;
using Steam.SteamInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Steam
{
    public class Client
    {
        private HttpClient HttpClient {get; set;}

        public Client()
        {
            HttpClient = new HttpClient();
        }

        public async Task<SteamHttpResponse> SendRequestAsync(SteamRequestBase request)
        {
            Debug.WriteLine("Requesting: " + request.Request);
            HttpResponseMessage response = await HttpClient.GetAsync(request.Request);

            return new SteamHttpResponse(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        }
        
        public async Task<FriendListResponse> GetFriendListAsync(FriendListRequest request)
        {
            return new FriendListResponse(await SendRequestAsync(request));
        }

        public async Task<PlayerBansResponse> GetPlayerBansAsync(PlayerBansRequest request)
        {
            return new PlayerBansResponse(await SendRequestAsync(request));
        }

        public async Task<PlayerSummariesResponse> GetPlayerSummariesAsync(PlayerSummariesRequest request)
        {
            return new PlayerSummariesResponse(await SendRequestAsync(request));
        }

        public async Task<OwnedGamesResponse> GetOwnedGamesAsync(OwnedGamesRequest request)
        {
            return new OwnedGamesResponse(await SendRequestAsync(request));
        }

        public async Task<FamilyShareAccountResponse> IsPlayingSharedGameAsync(FamilyShareAccountRequest request)
        {
            return new FamilyShareAccountResponse(await SendRequestAsync(request));
        }
    }
}
