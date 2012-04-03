using System;
using System.Threading;
using BadaBingBot.Api;

namespace BadaBingBot.Test.Fakes
{
    public class ThreadMessage : MessageBase
    {
        private readonly AutoResetEvent evt = new AutoResetEvent(false);

        public WaitHandle WaitHandle
        {
            get { return evt; }
        }

        public void Signal()
        {
            evt.Set();
        }

        public ThreadMessage(object sender, string text = "")
            : base(sender, text)
        {
        }
    }


    public class SubThreadMessage : ThreadMessage
    {
        public SubThreadMessage(object sender, string text)
            : base(sender, text)
        {

        }
    }
}