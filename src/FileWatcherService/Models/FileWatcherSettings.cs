using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService.Models
{
    public class FileWatcherSettings
    {
        public string InputFolder { get; set; } = string.Empty;
        public string OutputFolder { get; set; } = string.Empty;
        public int RetryCount { get; set; }
        public int RetryInterval { get; set; }
        public int CheckInterval { get; set; }
    }
}
