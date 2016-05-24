using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Twilio;

namespace IPAnnouncer
{
    public class IPAnnouncer : ServiceBase
    {
		private TwilioRestClient twilio = new TwilioRestClient(ConfigurationManager.AppSettings["TwilioAccountId"], ConfigurationManager.AppSettings["TwilioAccountSecret"]);
        private string lastIPs = GetIPs();

        public IPAnnouncer()
        {
            ServiceName = "IPAnnouncer";
        }

        protected override void OnStart(string[] args)
        {
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;

            Announce("Now online");
        }

        void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            if (lastIPs == (lastIPs = GetIPs())) return;

            Announce("IP has changed");
        }

        private void Announce(string message)
        {
			twilio.SendMessage(ConfigurationManager.AppSettings["FromNumber"], ConfigurationManager.AppSettings["ToNumber"], Dns.GetHostName() + " " + message + ": " + lastIPs);
        }

        private static string GetIPs()
        {
            return string.Join(", ", Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).Select(ip => ip.ToString()));
        }

        protected override void OnStop()
        {
        }
    }
}
