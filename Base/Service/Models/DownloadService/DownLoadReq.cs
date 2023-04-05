using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.DownloadService
{
    public class DownLoadReq
    {
        public string Url { get; set; }
        public string Folder_path { get; set; }
        public int Begin { get; set; }
        public int End { get; set; }
        public int ThreadCount { get; set; } = 1;
    }
}
