using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace ASAtoABLEService
{
    [RunInstaller(true)]
    public partial class AsaAbleServiceInstaller : System.Configuration.Install.Installer
    {
        public AsaAbleServiceInstaller()
        {
            InitializeComponent();
        }

        private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
