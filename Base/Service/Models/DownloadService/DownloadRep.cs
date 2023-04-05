using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.DownloadService
{
    public class DownloadRep
    {
        public DownloadRep()
        {
            Errors = new List<Error>();
        }
        public void Calculator(int begin,int end)
        {
            this.SussesRate = 1 - (Convert.ToDecimal(this.Errors.Count) / (Convert.ToDecimal(end) - Convert.ToDecimal(begin)+1));
        }
        public void Calculator(int count)
        {
            this.SussesRate = 1 - (Convert.ToDecimal(this.Errors.Count) /  Convert.ToDecimal(count) );
        }
        public decimal SussesRate { get; set; }
        public List<Error> Errors { get; set; }
    }
    public class Error
    {
        public Error(string url, string filename)
        {
            this.url = url;
            this.filename = filename;
        }

        public string url { get; set; }
        public string filename { get; set; }
        
    }
}
