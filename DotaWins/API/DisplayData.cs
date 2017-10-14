using System;
using System.ComponentModel;

namespace DotaWins
{
    public sealed partial class PlayerDisplay
    {
        public class DisplayData 
        {
            public string ID { get; private set; }

            public string Name { get; private set; }
            public string SoloMMR { get; private set; }
            public string EstimateMMR { get; private set; }

            public float Winrate { get; private set; }
            public TimeSpan AverageDuration { get; private set; }
            public Match[] RecentMatches { get; private set; }
            public float AverageKills { get; private set; }
            public float AverageDeaths { get; private set; }
            public float AverageAssists { get; private set; }
            public float AverageXPM { get; private set; }
            public float AverageGPM { get; private set; }
            public float AverageHeroDamage { get; private set; }
            public float AverageTowerDamage { get; private set; }
            public float AverageHeroHealing { get; private set; }
            public float AverageLastHits { get; private set; }
            public int[] WinLosses { get; set; }

          

            public void ConsumeData(string id, Match[] recentMatches)
            {
                ID = id;

                ConsumeRecentMatches(recentMatches);
            }

            private void ConsumeRecentMatches(Match[] recentMatches)
            {
                if (recentMatches != null && recentMatches.Length > 0)
                {
                    RecentMatches = recentMatches;
                    WinLosses = new int[recentMatches.Length];

                    for (var i = 0; i < RecentMatches.Length; i++)
                    {
                        
                        WinLosses[i] = recentMatches[i].Won ? 1 : -1;
                    }

                   
                }
            }

            public void Clear()
            {
                ID = "";

                Name = "";
                SoloMMR = "";
                EstimateMMR = "";

                Winrate = 0;
                AverageDuration = TimeSpan.Zero;
                RecentMatches = null;
                AverageKills = 0;
                AverageDeaths = 0;
                AverageAssists = 0;
                AverageXPM = 0;
                AverageGPM = 0;
                AverageHeroDamage = 0;
                AverageTowerDamage = 0;
                AverageHeroHealing = 0;
                AverageLastHits = 0;
                WinLosses = null;

              
            }

          
        }
    }
}

