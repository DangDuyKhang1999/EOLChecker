using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOLChecker
{
    internal class FilesInfo
    {
        // Property to store the directory path
        public string StrFilePath { get; }

        // Property to store the checked state of the file
        public bool BIsPass { get; set; }

        // Property to store lines containing the search character
        public List<int> ArrLines { get; set; }

        // Constructor to initialize the FilesInfo object with directory and initial state
        public FilesInfo(string strFilePath, bool bIsPass, List<int> arrLines)
        {
            StrFilePath = strFilePath;
            BIsPass     = bIsPass;
            ArrLines    = arrLines;
        }
        // Constructor to initialize the FilesInfo object with directory and initial state
    }
}
