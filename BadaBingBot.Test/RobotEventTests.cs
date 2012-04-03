using System;
using System.Threading;
using BadaBingBot.Api;
using BadaBingBot.Test.Fakes;
using NSubstitute;
using NUnit.Framework;

namespace BadaBingBot.Test
{
    [TestFixture]
    public class RobotEventTests
    {
        private IRobot robot;
        private ILogger logger;

        [SetUp]
        public void Setup()
        {
            logger = Substitute.For<ILogger>();
            robot = new Robot(logger);
        }

        [Test]
        public void PublishMessageWithNoSubscribersShouldNotFail()
        {
            robot.Publish(new MessageBase(this, "Hello World!"));
        }

        [Test]
        public void PublishMessageToOneSubscriber()
        {
            robot.Subscribe<ThreadMessage>(msg => {
                Thread.Yield();
                msg.Signal();
            });

            var message = new ThreadMessage(this);
            robot.Publish(message);
            
            Assert.IsTrue(message.WaitHandle.WaitOne(200));
        }

        [Test]
        public void UnhandledExceptionsShouldBeLogged()
        {
            var ex = new Exception("Foobar!");

            robot.Subscribe<ThreadMessage>(msg => {
                throw ex;
            });

            var message = new ThreadMessage(this, "");
            robot.Publish(message);

            Assert.IsFalse(message.WaitHandle.WaitOne(200));
            logger.Received().Error(ex, "Unhandled exception thrown by a message subscriber.");
        }

        [Test]
        public void PublishMessageToMultipleSubscribers()
        {
            bool firstReceived = false;
            robot.Subscribe<ThreadMessage>(msg => {
                firstReceived = true;
            });

            bool secondReceived = false;
            robot.Subscribe<ThreadMessage>(msg => {
                Thread.Yield();
                secondReceived = true;
                msg.Signal();
            });

            var message = new ThreadMessage(this);
            robot.Publish(message);

            Assert.IsTrue(message.WaitHandle.WaitOne(200));
            Assert.IsTrue(secondReceived);
            Assert.IsTrue(firstReceived);
        }

        [Test]
        public void SubscribeMessagePolymorphically()
        {
            robot.Subscribe<ThreadMessage>(msg => {
                Thread.Yield();
                msg.Signal();
            });
            
            var message = new SubThreadMessage(this, "");
            robot.Publish(message);
            
            Assert.IsTrue(message.WaitHandle.WaitOne(200));
        }

        [Test]
        public void SubscriptionShouldNotBeWeakReference()
        {
            robot.Subscribe<ThreadMessage>(msg => {
                Thread.Yield();
                msg.Signal();
            });
            GC.Collect();

            var message = new ThreadMessage(this);
            robot.Publish(message);
            
            Assert.IsTrue(message.WaitHandle.WaitOne(200));
        }

        [Test]
        public void MessageShouldNotBeReceivedAfterUnsubscribe()
        {
            var token = robot.Subscribe<ThreadMessage>(msg => {
                throw new Exception("This should never happen!");
            });
            token.Dispose();
            
            var message = new SubThreadMessage(this, "");
            robot.Publish(message);

            Assert.IsFalse(message.WaitHandle.WaitOne(200));
            logger.DidNotReceive().Error(Arg.Any<Exception>(), Arg.Any<string>());
        }
    }
}
