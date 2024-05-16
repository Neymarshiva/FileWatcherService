using FileWatcherService.Models;
using FileWatcherService.Services;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileWatcherService
{
    public class Worker : BackgroundService
    {
        private readonly IFileProcessor _fileProcessor;
        private readonly FileWatcherSettings _settings;

        public Worker(IFileProcessor fileProcessor, IOptions<FileWatcherSettings> settings)
        {
            _fileProcessor = fileProcessor;
            _settings = settings.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var xmlFiles = Directory.GetFiles(_settings.InputFolder, "*.xml");

                foreach (var xmlFilePath in xmlFiles)
                {
                    if (File.Exists(xmlFilePath))
                    {
                        await _fileProcessor.ProcessFileAsync(xmlFilePath);
                    }
                }

                // Wait for a configured interval before checking again
                await Task.Delay(_settings.CheckInterval, stoppingToken);
            }
        }
    }
}
