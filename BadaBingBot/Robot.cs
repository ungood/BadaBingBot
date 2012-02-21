#region License
// Copyright 2011 Jason Walker
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
using System.Reactive.Subjects;
using BadaBingBot.Api;
using Common.Logging;
using Microsoft.Practices.ServiceLocation;

namespace BadaBingBot
{
    public class Robot : IRobot
    {
        private readonly ISubject<IMessage> messageQueue;
        private readonly IList<IDisposable> subscribers;

        public IConfig Config { get; private set; }
        public ILog Log { get; private set; }
        public IServiceLocator ServiceLocator { get; private set; }

        public Robot(IConfig config, IServiceLocator serviceLocator, ILog log)
        {
            Config = config;
            ServiceLocator = serviceLocator;
            Log = log;

            messageQueue = new Subject<IMessage>();
            subscribers = new List<IDisposable>();

            Subscribe<IMessage>(Test);
        }

        private void Test(IMessage obj)
        {
            var chat = obj as IChatMessage;
            if(chat == null)
                return;

            chat.Reply("ECHO: " + chat.Text);
        }

        public void Publish(IMessage message)
        {
            messageQueue.OnNext(message);
        }

        public IDisposable Subscribe<TMessage>(Action<TMessage> subscriber)
            where TMessage : IMessage
        {
             var handle = messageQueue.OfType<TMessage>().Subscribe(subscriber);
             subscribers.Add(handle);
             return handle;
        }

        public void ScheduleJob(TimeSpan interval, Action<IRobot> action)
        {
            var timer = Observable.Interval(interval);
            timer.Subscribe(x => action(this));
        }
    }
}
