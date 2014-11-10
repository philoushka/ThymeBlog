
namespace Thyme.Web.Models
{
    public class DiskSaveItem
    {
        public string FileName { get; set; }
        public string SubDirectory { get; set; }
        public byte[] FileContents { get; set; } //byte array because could be any file: image, binary, text file, etc.
    }
}