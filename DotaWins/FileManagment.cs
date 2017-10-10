#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;

#endregion

namespace DotaWins
{
    public static class FileManagement
    {
        private static string _serverLog;

        private static string _steamInstallPath;

        public static string ServerLog
        {
            get
            {
                if (_serverLog == null)
                {
                    foreach (var path in SteamAppDirectories.Where(path => File.Exists(path + "\\common\\dota 2 beta\\game\\dota\\server_log.txt")))
                    {
                        _serverLog = path + "\\common\\dota 2 beta\\game\\dota\\server_log.txt";
                        break;
                    }
                    if (_serverLog == null)
                    {
                        throw new Exception("ServerLog not found");
                    }
                }

                return _serverLog;
            }
        }

        private static string SteamInstallPath => _steamInstallPath ?? (_steamInstallPath = Registry.GetValue(
                                                      "HKEY_LOCAL_MACHINE\\SOFTWARE\\Valve\\Steam", "InstallPath",
                                                      "") as string);

        private static IEnumerable<string> SteamAppDirectories
        {
            get
            {
                var steamAppDirectories = new List<string> {SteamInstallPath + "\\steamapps"};

                var lines = File.ReadAllLines(SteamInstallPath + "\\steamapps\\libraryfolders.vdf");

                for (var i = 4; i < lines.Length - 1; i++)
                {
                    var index = lines[i].IndexOfNth("\"", 3);
                    steamAppDirectories.Add(
                        lines[i].Substring(index + 1, lines[i].Length - (index + 2)) + "\\steamapps");
                }

                return steamAppDirectories;
            }         
        }
           
        private static int IndexOfNth(this string str, string value, int nth = 1)
        {
            if (nth <= 0)
            {
                throw new ArgumentException("Can not find the zeroth index of substring in string. Must start with 1");
            }

            var offset = str.IndexOf(value);
            for (var i = 1; i < nth; i++)
            {
                if (offset == -1)
                {
                    return -1;
                }

                offset = str.IndexOf(value, offset + 1);
            }
            return offset;
        }
    }
}                       