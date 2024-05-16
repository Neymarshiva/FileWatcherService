using FileWatcherService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FileWatcherService.Utilities
{
    public class XmlParser : IXmlParser
    {
        public DocumentInfo Parse(string xmlFilePath)
        {
            var xdoc = XDocument.Load(xmlFilePath);
            return new DocumentInfo
            {
                Mrn = xdoc.Root.Element("MRN").Value,
                DocumentType = xdoc.Root.Element("DocumentType").Value,
                EncounterDate = DateTime.Parse(xdoc.Root.Element("EncounterDate").Value)
            };
        }
    }

}
