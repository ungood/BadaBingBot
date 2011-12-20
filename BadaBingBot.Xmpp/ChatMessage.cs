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
using BadaBingBot.Api;
using agsXMPP;
using agsXMPP.protocol.client;

namespace BadaBingBot.Xmpp
{
    public class ChatMessage : ChatMessageBase
    {
        private readonly XmppClientConnection client;

        public Jid From { get; private set; }
        public bool IsGroupChat { get; private set; }

        public ChatMessage(XmppClientConnection client, Message message)
        {
            this.client = client;

            Text = message.Body;
            From = message.From;
            IsGroupChat = message.Type == MessageType.groupchat;
            Username = IsGroupChat
                ? message.From.Resource
                : message.From.User;
        }

        public override void Reply(string text, bool replyPrivate = false)
        {
            var to = IsGroupChat
                ? (replyPrivate ? From.ToString() : From.User + "@" + From.Server)
                : From.ToString();

            var type = IsGroupChat && !replyPrivate
                ? MessageType.groupchat
                : MessageType.chat;

            client.Send(new Message(to, type, text));
        }
    }
}
