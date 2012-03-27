using BadaBingBot.Api;

namespace BadaBingBot.Jenkins
{
    public class BuildMessage : MessageBase
    {
        public string JobName { get; set; }
        public string Result { get; set; }
        
        public BuildMessage(object sender)
            : base(sender)
        {
        }
    }
}
