using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PlotToPDFService
{
    public partial class Plot2PDFService : ServiceBase
    {
        private FileSystemWatcher _folderWatcher = null;

        // change the folder path to watch
        private static String _folderPath = Properties.Settings.Default.DrawingFolder;

        private static EventLog _log;
        string logsource = "Plot2PDFService";

        public Plot2PDFService()
        {
            InitializeComponent();

            // Initial setup to get started
            Autodesk.AcadIOUtils.SetupAutoCADIOContainer(Properties.Settings.Default.AutoCADIOClientId, Properties.Settings.Default.AutoCADIOClientSecret);

            // Bucket name that we will use in Amazon S3 for uploading our drawings 
            Autodesk.GeneralUtils.S3BucketName = Properties.Settings.Default.S3BucketName;

            // Create a new FileSystemWatcher and set its properties.
            _folderWatcher = new FileSystemWatcher();
            _folderWatcher.Path = _folderPath;

            // Watch for changes in LastAccess and LastWrite times, and
            // the renaming of files or directories. 
            _folderWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            // Only watch drawing files.
            _folderWatcher.Filter = "*.dwg";

            // Add event handlers.
            _folderWatcher.Created += new FileSystemEventHandler(OnChanged);
            
            // Begin watching.
            _folderWatcher.EnableRaisingEvents = true;
        }

        // Define the event handlers. 
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                _log.WriteEntry(String.Format("Drawing file was created in watched folder - {0}", e.FullPath));

                // Step 1 : Upload the drawing to S3 storage
                String hostDwgS3Url = Autodesk.GeneralUtils.UploadDrawingToS3(e.FullPath);

                if (String.IsNullOrEmpty(hostDwgS3Url))
                    return;

                _log.WriteEntry(String.Format("Uploaded drawing File to Amazon S3"));

                // Step 2 : Submit a AutoCADIO Workitem using the activity id
                String resulturl = Autodesk.AcadIOUtils.SubmitWorkItem("PlotToPDF", hostDwgS3Url);

                // Step 3 : Display the result in a web browser and download the result
                if (String.IsNullOrEmpty(resulturl) == false)
                {
                    _log.WriteEntry(String.Format("Result url from AutoCAD IO : {0}", resulturl));

                    String localFilePath = String.Empty;
                    if (Autodesk.GeneralUtils.Download(resulturl, ref localFilePath))
                    {
                        if (!String.IsNullOrEmpty(localFilePath))
                        {
                            _log.WriteEntry(String.Format("Result downloaded to {0}", localFilePath));

                            // Copy to the same folder as the one being watched
                            if (File.Exists(localFilePath))
                            {
                                String destPath = Path.Combine(_folderPath, Path.GetFileNameWithoutExtension(e.FullPath) + Path.GetExtension(localFilePath));
                                File.Copy(localFilePath, destPath);
                                _log.WriteEntry(String.Format("File moved to watched folder {0}", destPath));
                            }
                        }
                    }
                    else
                        _log.WriteEntry("Could not download result from url");
                }
                else
                    _log.WriteEntry("Workitem result url is empty !");
            }
        }



        protected override void OnStart(string[] args)
        {
            // Set up logging
            if (!System.Diagnostics.EventLog.SourceExists(logsource))
            {
                System.Diagnostics.EventLog.CreateEventSource(logsource, "Application");
            }
            _log = new EventLog();
            _log.Source = logsource;
            _log.Log = "Application";

            _log.WriteEntry(string.Format("Starting PlotToPDF service - {0}", "OnStart"), EventLogEntryType.Information);
        }

        protected override void OnStop()
        {
            _log.WriteEntry(string.Format("Stopping PlotToPDF service - {0}", "OnStop"), EventLogEntryType.Information);
        }
    }
}
