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
using BadaBingBot.Api;
using BadaBingBot.Xmpp.Config;
using agsXMPP;
using agsXMPP.Xml.Dom;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.disco;
using agsXMPP.protocol.x.muc;

namespace BadaBingBot.Xmpp
{
    public class XmppPluginInstance : IPluginInstance
    {
        private readonly XmppClientConnection client;
        private readonly XmppConfig config;
        private readonly MucManager chatManager;
        private readonly IRobot robot;
        private readonly ILogger logger;

        private readonly HashSet<string> ignoredJids = new HashSet<string>(); 

        public XmppPluginInstance(XmppConfig config, IRobot robot, ILogger logger)
        {
            this.config = config;
            this.robot = robot;
            this.logger = logger;

            client = new XmppClientConnection {
                Resource = config.Resource,
                Server = config.Server,
                Port = config.Port,
                Username = config.Username,
                Password = config.Password,
                UseSSL = config.UseSSL,
                UseStartTLS = config.UseStartTLS,
                KeepAlive = true,
                AutoPresence = true,
                AutoAgents = true,
                AutoRoster = true,
            };

            chatManager = new MucManager(client);

            client.OnAuthError += OnAuthError;
            client.OnError += OnError;
            client.OnLogin += OnLogin;
            client.OnPresence += OnPresence;
            client.OnMessage += OnMessage;
            client.OnIq += ClientOnOnIq;
            
            client.DiscoInfo.AddFeature(new DiscoFeature("urn:xmpp:bob"));
        }

        private void ClientOnOnIq(object sender, IQ iq)
        {
            if(iq.Type != IqType.get)
                return;

            var data = iq.FirstChild;
            if(data == null || data.TagName != "data")
                return;

            if(!data.HasAttribute("cid"))
                return;

            var cid = data.GetAttribute("cid");

        }

        public void Load()
        {
            client.Open();
        }

        public void Unload()
        {
            client.Close();
        }

        private void AddIgnore(string ignoredJid)
        {
            ignoredJids.Add(ignoredJid.ToLowerInvariant());
        }

        private void OnError(object sender, Exception ex)
        {
            logger.Error("Unhandled XMPP Error", ex);
        }

        private void OnAuthError(object sender, Element element)
        {
            logger.Error("Could not connect: {0}", element.ToString());
        }

        private void OnLogin(object sender)
        {
            logger.Debug("XMPP login success");
            client.Status = "BadaBingBot!";
            client.Show = ShowType.chat;
            client.SendMyPresence();
            AddIgnore(client.MyJID.ToString());

            foreach (var room in config.Rooms)
            {
                var nickname = room.Nickname ?? config.Username;
                var roomJid = new Jid(room.Jid);
                AddIgnore(room.Jid + "/" + nickname);

                chatManager.JoinRoom(roomJid, nickname, room.Password, true);
                chatManager.AcceptDefaultConfiguration(roomJid);

                robot.Subscribe(msg => NotifyRoomOfMessage(roomJid, msg));
            }
        }

        private void NotifyRoomOfMessage(Jid roomJid, IMessage message)
        {
            var notification = new Message(roomJid) {
                Body = message.Text
            };

            //client.Send(notification);
        }

        private void OnPresence(object sender, Presence pres)
        {
            logger.Debug("Presence received from {0} [{1}]: {2}",
                pres.From, pres.Type, pres.Status);

            if(pres.Type == PresenceType.subscribe)
                client.PresenceManager.ApproveSubscriptionRequest(pres.From);
        }

        private void OnMessage(object sender, Message msg)
        {
            if(string.IsNullOrEmpty(msg.Body)
                || msg.From == null
                || ignoredJids.Contains(msg.From.ToString().ToLowerInvariant()))
                return;

            logger.Debug("Message received from {0} [{1}]: {2}",
                msg.From, msg.Type, msg.Body);

            
            if(msg.Type == MessageType.chat || msg.Type == MessageType.groupchat)
            {
                var chatMessage = new ChatMessage(client, msg) {
                    Sender = this
                };
                robot.Publish(chatMessage);
            }
        }
    }
}
