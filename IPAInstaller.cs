using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace IPAnnouncer
{
    [RunInstaller(true)]
    public class IPAInstaller : Installer
    {
        public IPAInstaller()
        {
            var spi = new ServiceProcessInstaller();

            spi.Account = ServiceAccount.LocalSystem;

            var si = new ServiceInstaller();

            si.DisplayName = "IP Announcer";
            si.ServiceName = "IPAnnouncer";
            si.StartType = ServiceStartMode.Automatic;

            Installers.Add(spi);
            Installers.Add(si);
        }
    }
}
