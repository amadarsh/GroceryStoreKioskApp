using GroceryStoreKiosk.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStoreKiosk
{
    public class FileWatcher
    {
        private string folderLocation { get; set; }
        public FileWatcher(string folderLocation)
        {
            this.folderLocation = folderLocation;
        }

        /// <summary>
        /// This is the method which starts up the filewatcher
        /// </summary>
        public void Start()
        {
            FileSystemWatcher watcher = GetNewFileWatcher();
            while (true) ;
        }

        private FileSystemWatcher GetNewFileWatcher()
        {
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = this.folderLocation;
            // Watch for changes in LastAccess and LastWrite times, and the renaming of files or directories.
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = "*.txt";

            // Add event handlers.
            watcher.Created += new FileSystemEventHandler(OnNewScan);
            watcher.Renamed += new RenamedEventHandler(OnNewScan);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
            return watcher;
        }

        /// <summary>
        /// Event handler which gets executed when a new file gets created or a file gets renamed
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void OnNewScan(object source, FileSystemEventArgs e)
        {
            Console.Clear();
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("Scan received: " + e.FullPath);
            ProcessScannedItems(e.FullPath);            
        }

        /// <summary>
        /// This method parses the input scan file and calls the Kiosk checkout process
        /// </summary>
        /// <param name="fileName"></param>
        private static void ProcessScannedItems(string fileName)
        {
            List<ScannedItem> items = new List<ScannedItem>();
            // Read the file and map it line by line to the scannedItem object.
            try
            {
                var lines = Helper.ReadFromFile(fileName);
                lines.ForEach(l =>
                {
                    if (l != string.Empty)
                        items.Add(new ScannedItem { Name = l });
                });

                //Archive the file after processing
                Helper.ArchiveFile(fileName);

                //Call the checkout process
                if (items.Count > 0)
                {
                    Kiosk kiosk = new Kiosk(items);
                    kiosk.CheckOut();
                }
                else
                {
                    Console.WriteLine("No items in the scanned input file.");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("There is a problem with the CheckOut." + ex.Message);
                //Log the Exception to Kiosk Logger
            }
        }
    }
}
