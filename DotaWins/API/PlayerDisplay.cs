namespace DotaWins
{
    public partial class PlayerDisplay
    {
        public PlayerDisplay()
        {
            Data = new DisplayData();
        }

        public DisplayData Data { get; set; }

        public void Update(string playerID, int i)
        {
            Data.Clear();


            var recentMatches = OpenDotaApi.GetPlayerMatches(playerID,7);
            Data.ConsumeData(playerID,recentMatches);
          
        }
    }
}

