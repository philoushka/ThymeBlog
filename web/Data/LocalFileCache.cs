using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Thyme.Web.Models;

namespace Thyme.Web.Data
{
    public class LocalFileCache
    {
        public string LocalFileCachePath { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["BlogFilesDir"].ToString()); } }

        public bool HasItemsInCache()
        {
            try
            {
                return System.IO.Directory.GetFiles(LocalFileCachePath,"*",SearchOption.AllDirectories).Any();
            }
            catch (Exception) { return false; }
        }

        public void SaveLocalItem(SaveItem item)
        {
            Directory.CreateDirectory(Path.Combine(LocalFileCachePath,item.Dir));
            File.WriteAllBytes(Path.Combine(LocalFileCachePath,item.Dir, item.FileName), item.Contents);
        }

        public bool LocalItemExists(string dir, string fileName)
        {
            return File.Exists(Path.Combine(LocalFileCachePath, dir,fileName));
        }
    }

    public class SaveItem
    {
        public string FileName { get; set; }
        public string Dir { get; set; }
        public byte[] Contents { get; set; }
    }
}