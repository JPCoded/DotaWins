using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

        public static string GetLastLobby(string filePath)
        {
            var ServerLog = new List<string>();

            using (var fs = OpenStream(filePath, FileAccess.Read, FileShare.Read, 3))
            using (var sr = new StreamReader(fs, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    ServerLog.Add(sr.ReadLine());
                }
            }

            return ServerLog.Last(x => x.Contains("Lobby"));
        }

        private static FileStream OpenStream(string fileName, FileAccess fileAccess, FileShare fileShare, int retryCount)
        {
            while (true)
            {
                FileStream fs = null;
                for (var i = 1; i <= 3; ++i)
                {
                    try
                    {
                        fs = new FileStream(fileName, FileMode.Open, fileAccess, fileShare);
                        break;
                    }
                    catch (Exception)
                    {
                        if (i == 3)
                        {
                            throw;
                        }

                        Thread.Sleep(200);
                    }
                }

                if (fs.Length > 0)
                {
                    return fs;
                }
                if (retryCount > 0)
                {
                    Thread.Sleep(50);
                    retryCount--;
                    continue;
                }
                throw new Exception("File can't be read");
            }
        }
    } 
}
 