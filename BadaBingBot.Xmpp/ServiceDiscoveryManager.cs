using System;
using System.Collections.Generic;
using System.Linq;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.disco;

namespace BadaBingBot.Xmpp
{
    /// <summary>
    /// Given a chat server, discover the server that handles MUC.
    /// </summary>
    public class ServiceDiscvoryManager
    {
        private readonly Action<Jid> callback;
        private readonly DiscoManager discoManager;
        private readonly Queue<Jid> search; 

        public ServiceDiscvoryManager(XmppClientConnection client, Action<Jid> callback)
        {
            this.callback = callback;
            discoManager = new DiscoManager(client);

            search = new Queue<Jid>();
            search.Enqueue(new Jid(client.Server));
        }

        public void Find(string service)
        {
            
        }

        public void Search()
        {
            if(search.Count < 1)
            {
                callback(null);
                return;
            }

            var jid = search.Dequeue();
            discoManager.DiscoverInformation(jid, "http://jabber.org/protocol/muc", DiscoverInformationDone);
        }

        private void DiscoverInformationDone(object sender, IQ iq, object data)
        {
            var info = iq.Query as DiscoInfo;
            if(info == null)
                return;

            var identity = info.GetIdentities();
            var features = info.GetFeatures();

            if(features.Any(feature => feature.Var.EndsWith("muc")))
            {
                callback(iq.From);
                return;
            }

            discoManager.DiscoverItems(iq.From, DiscoverItemsDone);
        }

        private void DiscoverItemsDone(object sender, IQ iq, object data)
        {
            var discoItems = iq.Query as DiscoItems;
            if(discoItems == null)
                return;

            var items = discoItems.GetDiscoItems();
            foreach(var item in items)
                search.Enqueue(item.Jid);

            Search();
        }
    }
}