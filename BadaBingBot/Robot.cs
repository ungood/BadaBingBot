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
using System.Reactive.Subjects;
using BadaBingBot.Api;

namespace BadaBingBot
{
    public class Robot : IRobot
    {
        private readonly ILogger logger;
        private readonly ISubject<IMessage> messageQueue;
        private readonly IList<IDisposable> subscribers = new List<IDisposable>(); 

        public Robot(ILogger logger)
        {
            this.logger = logger;
            messageQueue = new Subject<IMessage>();
        }

        public void Publish(IMessage message)
        {
            logger.Debug("Message published: {0}", message.Text);
            messageQueue.OnNext(message);
        }

        public IDisposable Subscribe(Action<IMessage> subscriber)
        {
            var handle = messageQueue.Subscribe();
            subscribers.Add(handle);
            return handle;
        }

        public void ScheduleJob(TimeSpan interval, Action action)
        {
            var timer = Observable.Interval(interval);
            timer.Subscribe(x => action());
        }
    }
}
