using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadaBingBot.Api
{
    public interface IMessage
    {
        object Sender { get; }
        string Category { get; }
        string Text { get; }
    }
}
