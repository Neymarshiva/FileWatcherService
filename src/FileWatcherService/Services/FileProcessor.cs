using FileWatcherService.Models;
using FileWatcherService.Utilities;
using FileWatcherService.Validators;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService.Services
{
    public class FileProcessor : IFileProcessor
    {
        private readonly ILogger<FileProcessor> _logger;
        private readonly FileWatcherSettings _settings;
        private readonly IXmlParser _xmlParser;

        public FileProcessor(ILogger<FileProcessor> logger, IOptions<FileWatcherSettings> settings, IXmlParser xmlParser)
        {
            _logger = logger;
            _settings = settings.Value;
            _xmlParser = xmlParser;
        }

        public async Task ProcessFileAsync(string xmlFilePath)
        {
            string pdfFilePath = Path.ChangeExtension(xmlFilePath, ".pdf");
            for (int i = 0; i < _settings.RetryCount; i++)
            {
                if (File.Exists(pdfFilePath))
                {
                    var documentInfo = _xmlParser.Parse(xmlFilePath);
                    if (!MrnValidator.IsValid(documentInfo.Mrn))
                    {
                        _logger.LogError($"Invalid MRN format: {documentInfo.Mrn}");
                        return;
                    }

                    string newFileName = $"{documentInfo.Mrn}_{documentInfo.DocumentType}_{documentInfo.EncounterDate:dd-MM-yyyy}.pdf";
                    string newFilePath = Path.Combine(_settings.OutputFolder, newFileName);

                    File.Copy(pdfFilePath, newFilePath);
                    File.Delete(xmlFilePath);
                    File.Delete(pdfFilePath);

                    _logger.LogInformation($"Processed and moved {newFileName}");
                    return;
                }

                await Task.Delay(_settings.RetryInterval);
            }

            _logger.LogError($"PDF file not found for XML: {xmlFilePath}");
        }
    }


}
