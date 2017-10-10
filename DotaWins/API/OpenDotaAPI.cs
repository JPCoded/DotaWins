using Newtonsoft.Json;

namespace DotaWins
{
    public static class OpenDotaAPI
    {
        public static PlayerData GetPlayerData(string playerID)
        {
            var result = RequestHandler.GET($"https://api.opendota.com/api/players/{playerID}");

            return result != null ? JsonConvert.DeserializeObject<PlayerData>(result) : null;
        } 
             
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
 