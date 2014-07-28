using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
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
                return System.IO.Directory.GetFiles(LocalFileCachePath, "*", SearchOption.AllDirectories).Any();
            }
            catch (Exception) { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void SaveLocalItem(SaveItem item)
        {
            FileInfo writeToFile = new FileInfo(Path.Combine(LocalFileCachePath, item.SubDirectory, item.FileName));
            Directory.CreateDirectory(writeToFile.DirectoryName);
            File.WriteAllBytes(writeToFile.FullName, item.FileContents);
        }

        public bool LocalItemExists(string subDir, string fileName)
        {
            return File.Exists(Path.Combine(LocalFileCachePath, subDir, fileName));
        }

        public FileInfo GetItemOnDisk(string fileNameStartsWith)
        {
            string file = System.IO.Directory.GetFiles(LocalFileCachePath, "{0}.*".FormatWith(fileNameStartsWith), SearchOption.TopDirectoryOnly).FirstOrDefault();
            if (file.HasValue())
            {
                return new FileInfo(file);
            }
            return null;

        }

        public Dictionary<string,string> ListItemsOnDisk()
        {
            var files = new Dictionary<string,string>();
            foreach (FileInfo file in Directory.GetFiles(LocalFileCachePath, "*.md", SearchOption.TopDirectoryOnly).Select(x=>new FileInfo(x)))
            {
                files.Add(file.Name, ReadFileContents(file.Name));
            }
            return files;
        }

        public string ReadFileContents(string fileName)
        {
            return File.ReadAllText(Path.Combine(LocalFileCachePath, fileName));
        }
    }

    public class SaveItem
    {
        public string FileName { get; set; }
        public string SubDirectory { get; set; }
        public byte[] FileContents { get; set; } //byte array because could be any file: image, binary, text file, etc.
    }
}