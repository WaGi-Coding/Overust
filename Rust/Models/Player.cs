using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rust.Models
{
    public class Player
    {
        public string Name {get; set;}
        public ulong SteamID {get; set;}
        public string IPAddress {get; set;}
        public int ConnectedDuration {get; set;}
        public int Port {get; set;}
        public int Ping {get; set;}

        public Player(){}

        public static Player Parse(string str)
        {
            Player player = new Player();

            for(int iChar = 0, iResult = 0, iBuffer = 0; iChar < str.Length; ++iChar)
            {
                switch(iResult)
                {
                    case 0: // End SteamID
                        if (str[iChar] == ' ') {
                            player.SteamID = UInt64.Parse(str.Substring(iBuffer, iChar));
                            iBuffer = iChar + 1;
                            ++iResult;
                        }
                        break;

                    case 1: // Name
                        if (str[iChar] == '"') {
                            player.Name = ParsePlayerName(str.Substring(iChar + 1, 34));
                            iChar += 35;
                            iBuffer = iChar;
                            ++iResult;
                        }
                        break;
                        
                    case 2: // Start Ping
                        if (str[iChar] != ' ') {
                            iBuffer = iChar;
                            ++iResult;
                        }
                        break;

                    case 3: // End Ping
                        if (str[iChar] == ' ') {
                            var s = str.Substring(iBuffer, iChar - iBuffer);
                            player.Ping = int.Parse(str.Substring(iBuffer, iChar - iBuffer));
                            iBuffer = iChar + 1;
                            ++iResult;
                        }
                        break;

                    case 4: // Start ConnectedDuration
                        if (str[iChar] != ' ') {
                            iBuffer = iChar;
                            ++iResult;
                        }
                        break;

                    case 5: // End ConnectedDuration
                        if (str[iChar] == 's') {
                            player.ConnectedDuration = int.Parse(str.Substring(iBuffer, iChar - iBuffer));
                            iBuffer = iChar + 1;
                            ++iResult;
                        }
                        break;

                    case 6: // Start IpAddress/Port
                        if (str[iChar] != ' ') {
                            iBuffer = iChar;
                            ++iResult;
                        }
                        break;

                    case 7: // End Ipaddress/Port
                        if (str[iChar] == ':') {
                            player.IPAddress = str.Substring(iBuffer, iChar - iBuffer);
                            player.Port = int.Parse(str.Substring(iChar + 1));
                            ++iResult;
                        }
                        break;

                    default:
                        break;

                }
            }

            return player;
        }

        private static string ParsePlayerName(string str)
        {
            for(int iChar = str.Length - 1; iChar >= 0; --iChar)
            {
                if(str[iChar] == '"')
                {
                    var s = str.Substring(0, iChar);
                    return s;
                }
            }

            return "";
        }
    }
}
