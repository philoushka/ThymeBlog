using System;
using System.IO;
using System.Web.Hosting;

namespace Thyme.Web.Helpers
{
    public class Fingerprint
    {
        public static string Tag(string rootRelativePath)
        {
            string absolute = HostingEnvironment.MapPath("~" + rootRelativePath);
            DateTime date = File.GetLastWriteTime(absolute);
            int index = rootRelativePath.LastIndexOf('/');
            return rootRelativePath.Insert(index, "/v-" + date.Ticks);
        }
    }
}
