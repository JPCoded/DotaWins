using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotaWins
{
    public sealed partial class PlayerDisplay
    {
        public PlayerDisplay()
        {
            Data = new DisplayData();
        }
        public event EventHandler RetrievalStarted;

        public event EventHandler RetrievalCompleted;
        public DisplayData Data { get; set; }
 
        private CancellationTokenSource CTSource { get; set; }
        public async Task UpdateAsync(string playerId, int lobby)
        {
            Data.Clear();
            Match[] recentMatches = null;

            CTSource?.Cancel();
            CTSource = new CancellationTokenSource();

            var cancelToken = CTSource.Token;

            RetrievalStarted?.Invoke(this, null);

           
            await Task.Factory.StartNew(() =>
            {
                if (!cancelToken.IsCancellationRequested)
                {
                  //  playerData = OpenDotaAPI.GetPlayerData(playerID);
                }

                if (!cancelToken.IsCancellationRequested)
                {
                    recentMatches = OpenDotaApi.GetPlayerMatches(playerId,lobby);
                }

                if (!cancelToken.IsCancellationRequested)
                {
                    Data.ConsumeData(playerId, recentMatches);
                }
            }, cancelToken).ContinueWith(t => RetrievalCompleted?.Invoke(this, null), cancelToken);
         //   var recentMatches = OpenDotaApi.GetPlayerMatches(playerId,lobby);
            Data.ConsumeData(playerId,recentMatches);
          
        }
    }
}

