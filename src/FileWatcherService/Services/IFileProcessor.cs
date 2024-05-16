using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService.Services
{
    public interface IFileProcessor
    {
        Task ProcessFileAsync(string xmlFilePath);
    }
}
