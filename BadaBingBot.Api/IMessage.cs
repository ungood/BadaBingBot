using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadaBingBot.Api
{
    public interface IMessage
    {
        object Sender { get; }
        string Text { get; }
    }
}
