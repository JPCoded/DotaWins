namespace DotaWins
{
    public class PlayerData
    {
        public string tracked_until { get; set; }
        public string solo_competitive_rank { get; set; }
        public object competitive_rank { get; set; }
        public MmrEstimate mmr_estimate { get; set; }
        public Profile profile { get; set; }
    }
}