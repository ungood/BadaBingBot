namespace BadaBingBot.Api
{
    public interface IMessage
    {
        object Sender { get; }
        string Text { get; }
    }

    public class MessageBase : IMessage
    {
        public object Sender { get; private set; }
        public string Text { get; set; }

        public MessageBase(object sender, string text = "")
        {
            Sender = sender;
            Text = text;
        }
    }
}
