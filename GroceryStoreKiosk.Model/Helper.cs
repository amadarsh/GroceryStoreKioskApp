using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStoreKiosk.Model
{
    public class Helper
    {
        public static List<string> ReadFromFile(string fileName)
        {
            string line;
            var lines = new List<string>();
            using (StreamReader file = new StreamReader(fileName))
            {
                while ((line = file.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            return lines;
        }
        public static void DeleteFile(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            fi.Delete();
        }

        public static void ArchiveFile(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            fi.MoveTo(ConfigurationSettings.AppSettings["archiveFileCatalogLocation"].ToString() + "\\" 
                + fi.Name.Replace(fi.Extension, string.Empty) + "_"
                + DateTime.Now.Ticks.ToString() 
                + fi.Extension);
        }
    }
}
