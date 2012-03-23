using System;
using BadaBingBot.Api;

namespace BadaBingBot.Events
{
    public interface IEventAggregator<TPayload>
    {
        void Publish(TPayload payload);
        IDisposable Subscribe(Action<TPayload> subscriber);
    }
}
