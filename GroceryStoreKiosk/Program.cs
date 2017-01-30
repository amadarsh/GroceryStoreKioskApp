using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStoreKiosk
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Kiosk Started. Place the scan list..");
            //Set up the Filewatcher to monitor the folder where the scanned items are placed
            string inputFolderLocation = ConfigurationSettings.AppSettings["inputFolderLocation"].ToString();
            FileWatcher fileWatcher = new FileWatcher(inputFolderLocation);
            //Start the filewatcher
            fileWatcher.Start();
        }
    }
}
