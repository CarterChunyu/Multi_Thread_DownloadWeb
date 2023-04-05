using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.DownloadService
{
    public class AdditionalReq
    {
        public List<Error> Errors { get; set; }
        public int ThreadCount { get; set; }
    }
}
