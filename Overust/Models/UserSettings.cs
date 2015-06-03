using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITK.ModelManager;
using ProtoBuf;

namespace Overust.Models
{
    [Model("UserSettings")]
    [ProtoContract]
    public class UserSettings
    {
        [ProtoMember(1)]
        public ConsoleSettings ConsoleSettings {get; set;}

        [ProtoMember(2)]
        public ChatSettings ChatSettings {get; set;}
        
        [ProtoMember(3)]
        public GeneralSettings GeneralSettings {get; set;}

        [ProtoMember(4)]
        public AutoModerationSettings AutoModerationSettings {get; set;}


        public UserSettings()
        {
            ConsoleSettings = new ConsoleSettings();
            ChatSettings = new ChatSettings();
            GeneralSettings = new GeneralSettings();
            AutoModerationSettings = new AutoModerationSettings();
        }
    }
}
