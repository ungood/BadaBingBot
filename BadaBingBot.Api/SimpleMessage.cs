using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadaBingBot.Api
{
    public class SimpleMessage : IMessage
    {
        public object Sender { get; private set; }
        public string Category { get; private set; }
        public string Text { get; private set; }

        public SimpleMessage(object sender, string category, string text)
        {
            Sender = sender;
            Category = category;
            Text = text;
        }
    }
}
