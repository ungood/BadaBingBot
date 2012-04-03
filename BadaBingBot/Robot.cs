#region License
// Copyright 2012 Jason Walker
// ungood@onetrue.name
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and 
// limitations under the License.
#endregion

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BadaBingBot.Api;
using Caliburn.Micro;

namespace BadaBingBot
{
    public class Robot : IRobot
    {
        private readonly ILogger logger;

        private readonly EventAggregator aggregator;
        private readonly TaskFactory taskFactory;

        public string Name { get; private set; }

        public Robot(IConfig config, ILogger logger)
        {
            Name = config == null || string.IsNullOrWhiteSpace(config.Name) ? "BadaBingBot" : config.Name;

            this.logger = logger;

            taskFactory = new TaskFactory();
            aggregator = new EventAggregator {
                PublicationThreadMarshaller = PublicationThreadMarshaller
            };
        }

        private void PublicationThreadMarshaller(Action action)
        {
            taskFactory.StartNew(() => {
                try
                {
                    action();
                }
                catch(TargetInvocationException ex)
                {
                    logger.Error(ex.InnerException, "Unhandled exception thrown by a message subscriber.");
                }
            });
        }

        public IDisposable Subscribe<TMessage>(Action<TMessage> callback)
            where TMessage : IMessage
        {
            var handler = new ActionHandler<TMessage>(callback);
            aggregator.Subscribe(handler);
            return new SubscriptionToken<TMessage>(aggregator, handler);
        }

        public void Publish<TMessage>(TMessage message) where TMessage : IMessage
        {
            aggregator.Publish(message);
        }

        public void ScheduleJob(TimeSpan interval, Action action)
        {
            var timer = Observable.Interval(interval);
            timer.Subscribe(x => action());
        }

        private class SubscriptionToken<TMessage> : IDisposable
        {
            private EventAggregator aggregator;
            private readonly IHandle<TMessage> handler;

            public SubscriptionToken(EventAggregator aggregator, IHandle<TMessage> handler)
            {
                this.aggregator = aggregator;
                this.handler = handler;
            }

            public void Dispose()
            {
                if(aggregator == null)
                    throw new ObjectDisposedException("This object has been disposed already");
                aggregator.Unsubscribe(handler);
                aggregator = null;
            }
        }

        private class ActionHandler<TMessage> : IHandle<TMessage>
        {
            private readonly Action<TMessage> action;

            public ActionHandler(Action<TMessage> action)
            {
                this.action = action;
            }

            public void Handle(TMessage message)
            {
                action(message);
            }
        }
    }
}
