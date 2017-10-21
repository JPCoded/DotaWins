#region

using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace DotaWins
{
    public sealed partial class PlayerDisplay
    {
        public PlayerDisplay()
        {
            Data = new DisplayData();
        }

        public DisplayData Data { get; set; }

        private CancellationTokenSource CtSource { get; set; }
        public event EventHandler RetrievalStarted;

        public event EventHandler RetrievalCompleted;

        public async Task UpdateAsync(string playerId, int lobby)
        {
            Data.Clear();
            Match[] recentMatches = null;

            CtSource?.Cancel();
            CtSource = new CancellationTokenSource();

            var cancelToken = CtSource.Token;

            RetrievalStarted?.Invoke(this, null);

            await Task.Factory.StartNew(() =>
            {
                if (!cancelToken.IsCancellationRequested)
                {
                    recentMatches = OpenDotaApi.GetPlayerMatches(playerId, lobby);
                }

                if (!cancelToken.IsCancellationRequested)
                {
                    Data.ConsumeRecentMatches(recentMatches);
                }
            }, cancelToken).ContinueWith(t => RetrievalCompleted?.Invoke(this, null), cancelToken);

        }
    }
}