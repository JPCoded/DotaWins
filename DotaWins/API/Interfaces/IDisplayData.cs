using System;
using System.Collections.Generic;

namespace DotaWins
{
    public interface IDisplayData
    {
        string ID { get; }
        string Name { get; }
        string SoloMMR { get; }
        string EstimateMMR { get; }
        float Winrate { get; }
        TimeSpan AverageDuration { get; }
        Match[] RecentMatches { get; }
        float AverageKills { get; }
        float AverageDeaths { get; }
        float AverageAssists { get; }
        float AverageXPM { get; }
        float AverageGPM { get; }
        float AverageHeroDamage { get; }
        float AverageTowerDamage { get; }
        float AverageHeroHealing { get; }
        float AverageLastHits { get; }
        int[] WinLosses { get; }
        List<float[]> GXPM { get; set; }
        List<float> GPM { get; set; }
        List<float> XPM { get; set; }
        List<double> Average20XPM { get; set; }
        void ConsumeRecentMatches(Match[] recentMatches);
    }
}