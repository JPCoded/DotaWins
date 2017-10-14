#region

using Newtonsoft.Json;

#endregion

namespace DotaWins
{
    internal static class OpenDotaApi
    {
        public static Match[] GetPlayerMatches(string playerID, int lobbyType)
        {
            var requestString = $@"https://api.opendota.com/api/players/{playerID}/matches?limit=5000";

            if (lobbyType > 0)
            {
                requestString += $@"&lobby_type={lobbyType}";
            }


            var result = RequestHandler.GET(requestString);

            return result != null ? JsonConvert.DeserializeObject<Match[]>(result) : null;
        }
    }
}