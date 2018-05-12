namespace ASAtoABLEService
{
    partial class ASAtoABLE
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AsaFileSystemWatcher = new System.IO.FileSystemWatcher();
            this.eventLog = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.AsaFileSystemWatcher)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog)).BeginInit();
            // 
            // AsaFileSystemWatcher
            // 
            this.AsaFileSystemWatcher.EnableRaisingEvents = true;
            this.AsaFileSystemWatcher.Filter = "*.dat";
            this.AsaFileSystemWatcher.NotifyFilter = ((System.IO.NotifyFilters)((((System.IO.NotifyFilters.FileName | System.IO.NotifyFilters.DirectoryName) 
            | System.IO.NotifyFilters.LastWrite) 
            | System.IO.NotifyFilters.LastAccess)));
            this.AsaFileSystemWatcher.Changed += new System.IO.FileSystemEventHandler(this.OnChanged);
            this.AsaFileSystemWatcher.Created += new System.IO.FileSystemEventHandler(this.OnChanged);
            // 
            // eventLog
            // 
            this.eventLog.Log = "Application";
            this.eventLog.Source = "ASAtoABLE";
            // 
            // ASAtoABLE
            // 
            this.ServiceName = "ASAtoABLE";
            ((System.ComponentModel.ISupportInitialize)(this.AsaFileSystemWatcher)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog)).EndInit();

        }

        #endregion

        private System.IO.FileSystemWatcher AsaFileSystemWatcher;
        private System.Diagnostics.EventLog eventLog;
    }
}
