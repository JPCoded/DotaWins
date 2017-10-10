using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotaWins
{
    public class PlayerDisplay
    {
        public PlayerDisplay()
        {
            Data = new DisplayData();
        }

        public DisplayData Data { get; set; }

        private CancellationTokenSource CTSource { get; set; }

        public event EventHandler RetrievalStarted;

        public event EventHandler RetrievalCompleted;

        public void Update(string playerID, int Lobby)
        {
            Data.Clear();

            Match[] recentMatches = null;

            CTSource?.Cancel();
            CTSource = new CancellationTokenSource();

            var cancelToken = CTSource.Token;

            RetrievalStarted?.Invoke(this, null);

            Task.Factory.StartNew(() =>
            {


                if (!cancelToken.IsCancellationRequested)
                {
                    recentMatches = OpenDotaAPI.GetPlayerMatches(playerID, Lobby);
                }

                if (!cancelToken.IsCancellationRequested)
                {
                    Data.ConsumeData(playerID, recentMatches);
                }
            }, cancelToken).ContinueWith(t => RetrievalCompleted?.Invoke(this, null), cancelToken);
        }
    }
}
