#region

using System;

#endregion

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
            public int[] WinLosses { get; private set; }

                               
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
                    float totalXPM = 0;
                    float totalGPM = 0;
                    float totalHeroDamage = 0;
                    float totalTowerDamage = 0;
                    float totalHeroHealing = 0;
                    float totalLastHits = 0;

                    RecentMatches = recentMatches;
                    WinLosses = new int[recentMatches.Length];

                    for (var i = 0; i < RecentMatches.Length; i++)
                    {
                        totalSeconds += recentMatches[i].duration;

                        totalKills += recentMatches[i].kills;
                        totalDeaths += recentMatches[i].deaths;
                        totalAssists += recentMatches[i].assists;
                        totalXPM += recentMatches[i].xp_per_min;
                        totalGPM += recentMatches[i].gold_per_min;
                        totalHeroDamage += recentMatches[i].hero_damage.GetValueOrDefault(0);
                        totalTowerDamage += recentMatches[i].tower_damage.GetValueOrDefault(0);
                        totalHeroHealing += recentMatches[i].hero_healing.GetValueOrDefault(0);
                        totalLastHits += recentMatches[i].last_hits;
                        WinLosses[i] = recentMatches[i].Won ? 1 : -1;
                        if (recentMatches[i].Won)
                        {
                            wonMatches++;
                        }
                    }

                    Winrate = wonMatches / recentMatches.Length;
                    var sumDuration = new TimeSpan(0, 0, totalSeconds);
                    AverageDuration = new TimeSpan(sumDuration.Ticks / recentMatches.Length);

                    AverageKills = totalKills / recentMatches.Length;
                    AverageDeaths = totalDeaths / recentMatches.Length;
                    AverageAssists = totalAssists / recentMatches.Length;
                    AverageXPM = totalXPM / recentMatches.Length;
                    AverageGPM = totalGPM / recentMatches.Length;
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
            }
        }
    }
}