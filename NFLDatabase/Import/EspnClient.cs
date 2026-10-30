using System.Net.Http.Json;

namespace NFLDatabase.Import
{
    public class EspnClient
    {
        private readonly HttpClient _http;

        public EspnClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<EspnTeam>> GetTeamsAsync()
        {
            var res = await _http.GetFromJsonAsync<EspnTeamsResponse>("teams");
            return res?.Sports.FirstOrDefault()?.Leagues.FirstOrDefault()?
                       .Teams.Select(t => t.Team).ToList() ?? new();
        }

        public async Task<List<EspnAthlete>> GetRosterAsync(string teamId)
        {
            var res = await _http.GetFromJsonAsync<EspnRosterResponse>($"teams/{teamId}/roster");

            return res?.Athletes.SelectMany(g => g.Items).ToList() ?? new();
        }
    }
}
