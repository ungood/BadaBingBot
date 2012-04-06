namespace BadaBingBot.Api.Serialization
{
    public interface IHandleUnknownAttributes
    {
        void Add(string attributeName, string attributeValue);
    }
}
