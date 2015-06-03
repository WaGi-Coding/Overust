using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steam.SteamInterface
{
    public abstract class SteamResponseBase<T>
    {
        private bool mIsSuccess;
        private T mResult;

        public SteamResponseBase(SteamHttpResponse steamResponse)
        {
            mIsSuccess = steamResponse.IsSuccess;

            if(this.IsSuccess)
                mResult = Parse(steamResponse.Result);
        }

        public bool IsSuccess
        {
            get {return mIsSuccess;}
        }

        public T Result
        {
            get {return mResult;}
        }

        protected abstract T Parse(string result);
    }
}
