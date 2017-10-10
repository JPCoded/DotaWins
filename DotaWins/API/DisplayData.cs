﻿using System;
using System.ComponentModel;
using DotaWins.Outputs;


namespace DotaWins.API
{
    public partial class PlayerDisplay
    {
        public class DisplayData : INotifyPropertyChanged
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

            public event PropertyChangedEventHandler PropertyChanged;

            public void ConsumeData(string id, PlayerData playerData, Match[] recentMatches)
            {
                ID = id;

                NotifyUpdateLocal();

                ConsumePlayerData(playerData);
                ConsumeRecentMatches(recentMatches);
            }

            private void ConsumePlayerData(PlayerData playerData)
            {
                if (playerData?.profile == null)
                {
                    Name = "Anonymous";
                    SoloMMR = "X";
                    EstimateMMR = "X";
                }
                else
                {
                    Name = playerData.profile.personaname;
                    SoloMMR = playerData.solo_competitive_rank ?? "X";
                    EstimateMMR = playerData.mmr_estimate.estimate.HasValue
                        ? playerData.mmr_estimate.estimate.ToString()
                        : "X";
                }

                NotifyUpdateProfile();
            }

            private void ConsumeRecentMatches(Match[] recentMatches)
            {
                if (recentMatches != null && recentMatches.Length > 0)
                {
                    RecentMatches = recentMatches;

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

                    foreach (var match in recentMatches)
                    {
                        if (match.Won)
                        {
                            wonMatches++;
                        }

                        totalSeconds += match.duration;

                        totalKills += match.kills;
                        totalDeaths += match.deaths;
                        totalAssists += match.assists;
                        totalXPM += match.xp_per_min;
                        totalGPM += match.gold_per_min;
                        totalHeroDamage += match.hero_damage.GetValueOrDefault(0);
                        totalTowerDamage += match.tower_damage.GetValueOrDefault(0);
                        totalHeroHealing += match.hero_healing.GetValueOrDefault(0);
                        totalLastHits += match.last_hits;
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

                    NotifyUpdateMatches();
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

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }

            private void NotifyUpdateLocal()
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
            }

            private void NotifyUpdateProfile()
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EstimateMMR)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SoloMMR)));
            }

            private void NotifyUpdateMatches()
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Winrate)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageDuration)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RecentMatches)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageKills)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageDeaths)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageAssists)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageXPM)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageGPM)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageHeroDamage)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageTowerDamage)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageHeroHealing)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageLastHits)));
            }
        }
    }
}
