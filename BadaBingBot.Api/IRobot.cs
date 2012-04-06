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

namespace BadaBingBot.Api
{
    public interface IRobot
    {
        string Name { get; }

        void ScheduleJob(TimeSpan interval, Action action);

        void Publish<TMessage>(IPlugin source, TMessage message)
            where TMessage : Message;

        IDisposable Subscribe<TMessage>(Action<TMessage> subscriber, params MessageFilter[] filters)
            where TMessage : Message;
    }
}
