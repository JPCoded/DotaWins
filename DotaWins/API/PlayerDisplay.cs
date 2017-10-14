namespace DotaWins
{
    public sealed partial class PlayerDisplay
    {
        public PlayerDisplay()
        {
            Data = new DisplayData();
        }

        public DisplayData Data { get; set; }

        public void Update(string playerId, int lobby)
        {
            Data.Clear();


            var recentMatches = OpenDotaApi.GetPlayerMatches(playerId,lobby);
            Data.ConsumeData(playerId,recentMatches);
          
        }
    }
}

