﻿#region

using System.IO;
using System.Net;
using System.Text;
using System.Threading;

#endregion

namespace DotaWins
{
    internal static class RequestHandler
    {
        private const int MaxRetries = 3;

        public static string GET(string url) => GET(url, 0);

        private static string GET(string url, int retries)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            try
            {
                var response = request.GetResponse();
                using (var responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if ((ex.Response as HttpWebResponse)?.StatusCode is (HttpStatusCode) 429 && retries <= MaxRetries)
                {
                    Thread.Sleep(1000 * (retries + 1));
                    return GET(url, retries + 1);
                }

                return null;
            }
        }
    }
}