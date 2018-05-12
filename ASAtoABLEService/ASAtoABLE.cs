using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Text;

using log4net;

namespace ASAtoABLEService
{
    public partial class ASAtoABLE: ServiceBase
    {
        ILog log;

        AsaFileImportController asaFileImportController = null;
        public ASAtoABLE(ILog logger)
        {
            InitializeComponent();
            this.log = logger;
            asaFileImportController = new AsaFileImportController(logger);
        }

        /// <summary>
        /// The function is executed when a Start command is sent to the 
        /// service by the SCM or when the operating system starts (for a 
        /// service that starts automatically). It specifies actions to take 
        /// when the service starts. In this code sample, OnStart logs a 
        /// service-start message to the Application log, and queues the main 
        /// service function for execution in a thread pool worker thread.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <remarks>
        /// A service application is designed to be long running. Therefore, 
        /// it usually polls or monitors something in the system. The 
        /// monitoring is set up in the OnStart method. However, OnStart does 
        /// not actually do the monitoring. The OnStart method must return to 
        /// the operating system after the service's operation has begun. It 
        /// must not loop forever or block. To set up a simple monitoring 
        /// mechanism, one general solution is to create a timer in OnStart. 
        /// The timer would then raise events in your code periodically, at 
        /// which time your service could do its monitoring. The other 
        /// solution is to spawn a new thread to perform the main service 
        /// functions, which is demonstrated in this code sample.
        /// </remarks>
        protected override void OnStart(string[] args)
        {
            // Log a service start message to the Application log. 
            eventLog.WriteEntry("Start " + this.ServiceName + " Service");

            AsaFileSystemWatcher.Path = ConfigurationManager.AppSettings["FileSystemWatcherPath"];
            log.Info("File System Watcher Path is " + AsaFileSystemWatcher.Path);
        }

        /// <summary>
        /// The function is executed when a Stop command is sent to the 
        /// service by SCM. It specifies actions to take when a service stops 
        /// running. In this code sample, OnStop logs a service-stop message 
        /// to the Application log, and waits for the finish of the main 
        /// service function.
        /// </summary>
        protected override void OnStop()
        {
            // Log a service stop message to the Application log. 
            eventLog.WriteEntry("Stop "+ this.ServiceName + " Service");
        }

        /* DEFINE WATCHER EVENTS... */

        /// <summary>
        /// Event occurs when the contents of a File or Directory is changed
        /// </summary>
        private void OnChanged(object sender, System.IO.FileSystemEventArgs e)
        {
            log.Info("Process Asa File");
            //code here for newly changed file or directory
            asaFileImportController.ProcessAsaFile(e.FullPath);

            log.Info("Archive Asa File");
            //Move file to Archive directory
            asaFileImportController.ArchiveAsaFile(e.FullPath);
        }
    }
}
