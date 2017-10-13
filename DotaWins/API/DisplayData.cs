using System;
using System.ComponentModel;

namespace DotaWins
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
            public int[] WinLosses { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;

            public void ConsumeData(string id, Match[] recentMatches)
            {
                ID = id;

                NotifyUpdateLocal();

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
                        WinLosses[i] = recentMatches[i].Won ? 1 : 0;
                    }

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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WinLosses)));
            }
        }
    }
}

