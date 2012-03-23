using System;
using System.Reactive.Subjects;

namespace BadaBingBot.Events
{
    public class RxEventAggregator<TBasePayload> : IEventAggregator<TBasePayload>
    {
        private readonly ISubject<TBasePayload> subject = new Subject<TBasePayload>();

        public void Publish(TBasePayload payload)
        {
            subject.OnNext(payload);
        }

        public IDisposable Subscribe<TPayload>(Action<TBasePayload> subscriber)
            where TPayload : TBasePayload
        {
            return subject.Subscribe(subscriber);
        }
    }
}
