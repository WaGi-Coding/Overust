using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steam.SteamInterface
{
    public class SteamRequestBase
    {
        private const string UnformattedRequest = @"https://api.steampowered.com/{0}/{1}/v000{2}/?{3}&format=json";

        private string mInterfaceName;
        private string mMethodName;
        private int mMethodVersion;
        private string mParameters;
        private bool mRequiresSteamAPIKey;
        private string mSteamAPIKey;

        public SteamRequestBase(string interfaceName, string methodName, int methodVersion, string steamAPIKey, params string[] parameters):
            this(interfaceName, methodName, methodVersion, parameters)
        {
            mSteamAPIKey = steamAPIKey;
            mRequiresSteamAPIKey = true;
        }

        public SteamRequestBase(string interfaceName, string methodName, int methodVersion, params string[] parameters)
        {
            mInterfaceName = interfaceName;
            mMethodName = methodName;
            mMethodVersion = methodVersion;
            mParameters = String.Join("&", parameters);
        }

        public string InterfaceName
        {
            get {return mInterfaceName;}
        }

        public string MethodName
        {
            get {return mMethodName;}
        }

        public int MethodVersion
        {
            get {return mMethodVersion;}
        }

        public bool RequiresSteamAPIKey
        {
            get {return mRequiresSteamAPIKey;}
        }

        public string SteamAPIKey
        {
            get {return mSteamAPIKey;}
        }

        public string Request
        {
            get
            {
                string formattedParams = String.Empty;

                if(this.RequiresSteamAPIKey){
                    formattedParams = String.Format("key={0}&", this.SteamAPIKey);
                }

                formattedParams += mParameters;

                return String.Format(UnformattedRequest, this.InterfaceName, this.MethodName, this.MethodVersion.ToString(), formattedParams);
            }
        }
    }
}
