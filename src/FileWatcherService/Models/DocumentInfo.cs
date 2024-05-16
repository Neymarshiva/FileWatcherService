using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService.Models
{
    public class DocumentInfo
    {
        public string Mrn { get; set; }
        public string DocumentType { get; set; }
        public DateTime EncounterDate { get; set; }
    }
}
