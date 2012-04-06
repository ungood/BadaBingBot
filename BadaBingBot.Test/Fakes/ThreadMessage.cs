using System;
using System.Threading;
using BadaBingBot.Api;

namespace BadaBingBot.Test.Fakes
{
    public class ThreadMessage : Message
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
    }


    public class SubThreadMessage : ThreadMessage
    {
    }
}