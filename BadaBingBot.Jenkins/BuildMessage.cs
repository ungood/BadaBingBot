using BadaBingBot.Api;

namespace BadaBingBot.Jenkins
{
    public class BuildMessage : Message
    {
        public string JobName
        {
            get { return this["jobName"]; }
            set { this["jobName"] = value; }
        }

        public string Result
        {
            get { return this["result"]; }
            set { this["result"] = value; }
        }

        public string Text
        {
            get { return this["text"]; }
            set { this["text"] = value; }
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
