using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadaBingBot.Events;
using NSubstitute;
using NUnit.Framework;

namespace BadaBingBot.Test
{
    public interface IDummyMessage
    {
        string Text { get; set; }
    }

    public class DummyMessageA : IDummyMessage
    {
        public string Text { get; set; }
    }

    public class DummyMessageB : IDummyMessage
    {
        public string Text { get; set; }
    }

    public interface IDummySubscriber<in TMessageType>
    {
        void ReceiveMessage(TMessageType type);
    }

    [TestFixture]
    public class RxEventAggregatorTests
    {
        private IEventAggregator<IDummyMessage> aggregator;

        [SetUp]
        public void Setup()
        {
            aggregator = new RxEventAggregator<IDummyMessage>();
        }
        
        [Test]
        public void PublishWithNoSubscribersShouldNotFail()
        {
            aggregator.Publish(new DummyMessageA {Text = "Foo"});
        }

        [Test]
        public void PublishShouldNotifyAllSubscribersOfSameType()
        {
            var subscriberA = Substitute.For<IDummySubscriber<DummyMessageA>>();

            aggregator.Subscribe<DummyMessageA>(msg => subscriberA.ReceiveMessage(msg));
        }
    }
}
