using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileWatcherService.Validators
{
    public static class MrnValidator
    {
        private static readonly Regex MrnRegex = new Regex(@"^\d{7}[A-Za-z]$", RegexOptions.Compiled);

        public static bool IsValid(string mrn)
        {
            return MrnRegex.IsMatch(mrn);
        }
    }

}
