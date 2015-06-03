using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steam.SteamInterface
{
    public class SteamHttpResponse
    {
        private bool mIsSuccess {get; set;}
        private string mResult {get; set;}

        public SteamHttpResponse(bool isSuccess, string result)
        {
            mIsSuccess = isSuccess;
            mResult = result;
        }

        public bool IsSuccess
        {
            get {return mIsSuccess;}
        }

        public string Result
        {
            get {return mResult;}
        }
    }
}
