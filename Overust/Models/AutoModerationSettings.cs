using ITK.WPF;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overust.Models
{
    [ProtoContract]
    public class AutoModerationSettings: Notifiable
    {
        private bool mIsPingModerationEnabled;
        private int mPingKickThreshold;
        private bool mIsChatModerationEnabled;
        private string mChatKickModerationString;
        private List<string> mChatKickModerationWords;
        private string mChatBanModerationString;
        private List<string> mChatBanModerationWords;

        [ProtoMember(1)]
        public bool IsPingModerationEnabled
        {
            get { return mIsPingModerationEnabled; }
            set { SetField(ref mIsPingModerationEnabled, value); }
        }
        
        [ProtoMember(2)]
        public int PingKickThreshold
        {
            get { return mPingKickThreshold; }
            set { SetField(ref mPingKickThreshold, value); }
        }
        
        [ProtoMember(3)]
        public bool IsChatModerationEnabled
        {
            get { return mIsChatModerationEnabled; }
            set { SetField(ref mIsChatModerationEnabled, value); }
        }

        [ProtoMember(4)]
        public string ChatKickModerationString
        {
            get { return mChatKickModerationString; }
            set { SetField(ref mChatKickModerationString, value); ChatKickModerationWords = value.Split(';').ToList(); }
        }

        public List<string> ChatKickModerationWords
        {
            get { return mChatKickModerationWords; }
            set { SetField(ref mChatKickModerationWords, value); }
        }
        
        [ProtoMember(5)]
        public string ChatBanModerationString
        {
            get { return mChatBanModerationString; }
            set { SetField(ref mChatBanModerationString, value); ChatBanModerationWords = value.Split(';').ToList(); }
        }

        public List<string> ChatBanModerationWords
        {
            get { return mChatBanModerationWords; }
            set { SetField(ref mChatBanModerationWords, value); }
        }
    }
}
