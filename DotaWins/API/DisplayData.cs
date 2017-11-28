#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace DotaWins
{
    public sealed partial class PlayerDisplay
    {
        public class DisplayData : IDisplayData
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
            public int[] WinLosses { get; private set; }
            public List<float[]> GXPM { get; set; }
            public List<float> GPM { get; set; }
            public List<float> XPM { get; set; }
            public List<double> Average20XPM { get; set; }
                          
            public void ConsumeRecentMatches(Match[] recentMatches)
            {
                Clear();
                if (recentMatches != null && recentMatches.Length > 0)
                {
                    float wonMatches = 0;
                    var totalSeconds = 0;
                    float totalKills = 0;
                    float totalDeaths = 0;
                    float totalAssists = 0;
                    float totalXpm = 0;
                    float totalGpm = 0;
                    float totalHeroDamage = 0;
                    float totalTowerDamage = 0;
                    float totalHeroHealing = 0;
                    float totalLastHits = 0;
                     
                    RecentMatches = recentMatches;
                    var matchesList = recentMatches.ToList();
                    var matches = new List<double>();
                    matchesList.ForEach(a=>matches.Add(a.xp_per_min));
                    Average20XPM = AvgClass.Average(matches, 20);
                    
                    WinLosses = new int[recentMatches.Length];
                    GXPM = new List<float[]>();
                    foreach (var recentMatch in RecentMatches)
                    {
                        totalSeconds += recentMatch.duration;

                        totalKills += recentMatch.kills;
                        totalDeaths += recentMatch.deaths;
                        totalAssists += recentMatch.assists;
                        GXPM.Add(new[]{ (float)recentMatch.gold_per_min,recentMatch.xp_per_min});
                        XPM.Add(recentMatch.xp_per_min);
                        GPM.Add(recentMatch.gold_per_min);
                        totalXpm += recentMatch.xp_per_min;
                        totalGpm += recentMatch.gold_per_min;
                        totalHeroDamage += recentMatch.hero_damage.GetValueOrDefault(0);
                        totalTowerDamage += recentMatch.tower_damage.GetValueOrDefault(0);
                        totalHeroHealing += recentMatch.hero_healing.GetValueOrDefault(0);
                        totalLastHits += recentMatch.last_hits;
                        if (recentMatch.Won)
                        {
                            wonMatches++;
                        }
                    }
                    for (var i = 0; i < RecentMatches.Length; i++)
                    {
                        WinLosses[i] = recentMatches[i].Won ? 1 : -1;                   
                    }

                    Winrate = wonMatches / recentMatches.Length;
                    var sumDuration = new TimeSpan(0, 0, totalSeconds);
                    AverageDuration = new TimeSpan(sumDuration.Ticks / recentMatches.Length);

                    AverageKills = totalKills / recentMatches.Length;
                    AverageDeaths = totalDeaths / recentMatches.Length;
                    AverageAssists = totalAssists / recentMatches.Length;
                    AverageXPM = totalXpm / recentMatches.Length;
                    AverageGPM = totalGpm / recentMatches.Length;
                    AverageHeroDamage = totalHeroDamage / recentMatches.Length;
                    AverageTowerDamage = totalTowerDamage / recentMatches.Length;
                    AverageHeroHealing = totalHeroHealing / recentMatches.Length;
                    AverageLastHits = totalLastHits / recentMatches.Length;
                }
            }

            internal void Clear()
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
                Average20XPM = null;
                XPM = null;
                GPM = null;
            }
        }
    }
}