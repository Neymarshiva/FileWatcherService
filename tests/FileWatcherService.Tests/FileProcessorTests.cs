using FileWatcherService.Models;
using FileWatcherService.Services;
using FileWatcherService.Utilities;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace FileWatcherService.Tests
{
    [TestFixture]
    public class FileProcessorTests
    {
        private Mock<IOptions<FileWatcherSettings>> _mockSettings;
        private FileWatcherSettings _settings;
        private Mock<IXmlParser> _mockXmlParser;
        private FileProcessor _fileProcessor;

        [SetUp]
        public void SetUp()
        {
            _settings = new FileWatcherSettings
            {
                InputFolder = "C:\\InputFolder",
                OutputFolder = "C:\\OutputFolder",
                RetryCount = 3,
                RetryInterval = 1000
            };
            _mockSettings = new Mock<IOptions<FileWatcherSettings>>();
            _mockSettings.Setup(s => s.Value).Returns(_settings);

            _mockXmlParser = new Mock<IXmlParser>();

            _fileProcessor = new FileProcessor(NullLogger<FileProcessor>.Instance, _mockSettings.Object, _mockXmlParser.Object);
        }

        [Test]
        public async Task ProcessFileAsync_FileExists_ShouldCopyAndDeleteFiles()
        {
            // Arrange
            var xmlFilePath = Path.Combine(_settings.InputFolder, "1234567A.xml");
            var pdfFilePath = Path.ChangeExtension(xmlFilePath, ".pdf");
            var newFilePath = Path.Combine(_settings.OutputFolder, "1234567A_Test_07-05-2024.pdf");

            var documentInfo = new DocumentInfo
            {
                Mrn = "1234567A",
                DocumentType = "Test",
                EncounterDate = new DateTime(2024, 5, 7)
            };

            _mockXmlParser.Setup(x => x.Parse(It.IsAny<string>())).Returns(documentInfo);

            // Simulate existing files
            Directory.CreateDirectory(_settings.InputFolder);
            Directory.CreateDirectory(_settings.OutputFolder);
            File.WriteAllText(xmlFilePath, "<root><MRN>1234567A</MRN><DocumentType>Test</DocumentType><EncounterDate>2024-05-07</EncounterDate></root>");
            File.WriteAllText(pdfFilePath, "PDF content");

            // Act
            await _fileProcessor.ProcessFileAsync(xmlFilePath); 

            // Cleanup
            File.Delete(newFilePath);
        }
    }
}
