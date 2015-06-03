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
    public class ConsoleSettings: Notifiable
    {
        private bool mIsAutoScrollEnabled;
        private bool mIsTextWrappingEnabled;
        private bool mIsTimestampingEnabled;
        private bool mShowPublicConsoleLog;
        private bool mIsLoggingEnabled;
        private bool mIsLogTimestampingEnabled;
        private bool mIsPublicConsoleLoggingEnabled;

        [ProtoMember(1)]
        public bool IsAutoScrollEnabled
        {
            get { return mIsAutoScrollEnabled; }
            set { SetField(ref mIsAutoScrollEnabled, value); }
        }

        [ProtoMember(2)]
        public bool IsTextWrappingEnabled
        {
            get { return mIsTextWrappingEnabled; }
            set { SetField(ref mIsTextWrappingEnabled, value); }
        }

        [ProtoMember(3)]
        public bool IsTimestampingEnabled
        {
            get { return mIsTimestampingEnabled; }
            set { SetField(ref mIsTimestampingEnabled, value); }
        }

        [ProtoMember(4)]
        public bool ShowPublicConsoleLog
        {
            get { return mShowPublicConsoleLog; }
            set { SetField(ref mShowPublicConsoleLog, value); }
        }
        
        [ProtoMember(5)]
        public bool IsLoggingEnabled
        {
            get { return mIsLoggingEnabled; }
            set { SetField(ref mIsLoggingEnabled, value); }
        }
        
        [ProtoMember(6)]
        public bool IsLogTimestampingEnabled
        {
            get { return mIsLogTimestampingEnabled; }
            set { SetField(ref mIsLogTimestampingEnabled, value); }
        }

        [ProtoMember(7)]
        public bool IsPublicConsoleLoggingEnabled
        {
            get { return mIsPublicConsoleLoggingEnabled; }
            set { SetField(ref mIsPublicConsoleLoggingEnabled, value); }
        }
    }
}
