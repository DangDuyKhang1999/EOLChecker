
using static EOLChecker.Form1;

namespace EOLChecker
{
    internal class EOLChecker
    {
        private readonly List<FilesInfo> FilesInfoList = new();
        private readonly List<FilesReplaceInfo> FilesReplaceInfoList = new();
        public LineEnding LineEnding { get; set; }
        public TextBox TxtBrowser { get; set; }

        public List<FilesInfo> GetFilesInfoList()
        {
            return FilesInfoList;
        }

        public List<FilesReplaceInfo> GetFilesReplaceInfoList()
        {
            return FilesReplaceInfoList;
        }
        public EOLChecker(TextBox txtBrowser)
        {
            LineEnding = LineEnding.None;
            TxtBrowser = txtBrowser;
        }

        public bool HasCodeFiles(string directoryPath)
        {
            bool iResult = false;

            // Check source files in the current directory
            foreach (var file in Directory.GetFiles(directoryPath, "*.h").Concat(Directory.GetFiles(directoryPath, "*.cpp").Concat(Directory.GetFiles(directoryPath, "*.cs"))))
            {
                List<int> ArrLines = new();
                List<int> ArrReplaceLines = new();
                bool iResultCheckFile = AllLinesEndWithLF(file, ArrLines, ArrReplaceLines);
                FilesInfoList.Add(new FilesInfo(file, iResultCheckFile, ArrLines));
                iResult = true;
                if (iResultCheckFile)
                {
                    FilesReplaceInfoList.Add(new FilesReplaceInfo(file, ArrReplaceLines));
                }
                else
                {
                    ArrReplaceLines.Clear();
                }
            }

            // Recursively check all subdirectories
            foreach (var subDirectory in Directory.GetDirectories(directoryPath))
            {
                iResult |= HasCodeFiles(subDirectory);
            }

            return iResult;
        }


        public bool PrintFilesName(RichTextBox rtxtResult)
        {
            Font fontPath = new Font(rtxtResult.Font, FontStyle.Bold);
            Font fontLine = new Font(rtxtResult.Font, FontStyle.Italic); // Thay đổi FontStyle nếu cần

            bool bResult = false;
            foreach (var fileInfo in FilesInfoList)
            {
                if (fileInfo.BIsPass)
                {
                    string linecode = "";
                    foreach (int line in fileInfo.ArrLines)
                    {
                        linecode += "   line: " + line + Environment.NewLine;
                        bResult = true;
                    }

                    // Sử dụng fontPath cho fileInfo.StrFilePath
                    rtxtResult.SelectionStart = rtxtResult.TextLength;
                    rtxtResult.SelectionColor = Color.FromArgb(162, 87, 114);
                    rtxtResult.SelectionLength = 0;
                    rtxtResult.SelectionFont = fontPath;
                    rtxtResult.SelectedText = fileInfo.StrFilePath + Environment.NewLine;

                    // Sử dụng fontLine cho linecode
                    rtxtResult.SelectionStart = rtxtResult.TextLength;
                    rtxtResult.SelectionColor = Color.Black;
                    rtxtResult.SelectionLength = 0;
                    rtxtResult.SelectionFont = fontLine;
                    rtxtResult.SelectedText = linecode;
                }
            }
            fontPath.Dispose();
            fontLine.Dispose();
            return bResult;
        }


        public bool AllLinesEndWithLF(string filePath, List<int> ArrLines, List<int> ArrReplaceLines)
        {
            char CR = '\r';
            char LF = '\n';
            bool bResult = false;
            switch (LineEnding)
            {
                case LineEnding.CRLF:
                    using (StreamReader reader = new(filePath))
                    {
                        int lineCount = 0;
                        int previousChar = '\0';
                        int index = 0;
                        bool bPreviousIsCR = false;
                        while (!reader.EndOfStream)
                        {
                            int currentChar = reader.Read();
                            //char charValue = (char)currentChar; // for debug
                            if (bPreviousIsCR && currentChar == LF)
                            {
                                bResult = true;
                                bPreviousIsCR = false;
                                lineCount++;
                                ArrLines.Add(lineCount);
                                ArrReplaceLines.Add(index);
                            }
                            else if (currentChar == CR && bPreviousIsCR)
                            {
                                lineCount++;
                            }
                            else if (bPreviousIsCR == false && currentChar == LF)
                            {
                                bPreviousIsCR = false;
                                lineCount++;
                            }
                            else if (currentChar == -1)
                            {
                                // Kết thúc nếu đọc hết tệp tin
                                break;
                            }
                            if (currentChar == CR)
                            {
                                bPreviousIsCR = true;
                            }
                            previousChar = currentChar;
                            index++;
                        }
                    }
                    break;

                case LineEnding.CR: //CR (\r) = 13
                    string content = String.Empty;
                    int lastindex = -99;
                    using (StreamReader readerLength = new(filePath))
                    {
                        content = readerLength.ReadToEnd();
                        lastindex = content.Length;
                    }
                    using (StreamReader reader = new(filePath))
                    {
                        int lineCount = 0;
                        int previousChar = '\0';
                        int index = 0;
                        bool bPreviousIsCR = false;
                        while (!reader.EndOfStream)
                        {
                            int currentChar = reader.Read();
                            char charValue = (char)currentChar;
                            // Kiểm tra xem ký tự có phải là '\r' không
                            if (currentChar != CR && bPreviousIsCR && currentChar != LF)
                            {
                                bResult = true;
                                bPreviousIsCR = false;
                                lineCount++;
                                ArrLines.Add(lineCount);
                                ArrReplaceLines.Add(index - 1);
                            }
                            else if (currentChar == CR && bPreviousIsCR)
                            {
                                lineCount++;
                                bResult = true;
                                ArrLines.Add(lineCount);
                                ArrReplaceLines.Add(index - 1);
                            }
                            else if (currentChar == LF)
                            {
                                lineCount++;
                                bPreviousIsCR = false;
                            }
                            else if (currentChar == -1)
                            {
                                // Kết thúc nếu đọc hết tệp tin
                                break;
                            }
                            if (currentChar == CR)
                            {
                                bPreviousIsCR = true;
                            }
                            if (index == lastindex - 1 && currentChar == CR)
                            {
                                lineCount++;
                                bResult = true;
                                ArrLines.Add(lineCount);
                                ArrReplaceLines.Add(index);
                            }
                            previousChar = currentChar;
                            index++;
                        }
                    }
                    break;

                case LineEnding.LF: //LF (\n) = 10
                    using (StreamReader reader = new(filePath))
                    {
                        int lineCount = 0;
                        int previousChar = '\0';
                        int index = 0;
                        bool bPreviousIsCR = false;
                        while (!reader.EndOfStream)
                        {
                            int currentChar = reader.Read();

                            // Kiểm tra xem ký tự có phải là '\n' không
                            if (currentChar == LF)
                            {
                                if (previousChar != CR)
                                {
                                    // In ra dòng hiện tại
                                    bResult = true;
                                    lineCount++;
                                    ArrLines.Add(lineCount);
                                    ArrReplaceLines.Add(index);
                                }
                            }
                            else if (currentChar == CR && bPreviousIsCR)
                            {
                                bPreviousIsCR = false;
                                lineCount++;
                            }
                            else if (currentChar == CR)
                            {
                                lineCount++;
                            }
                            else if (currentChar == -1)
                            {
                                // Kết thúc nếu đọc hết tệp tin
                                break;
                            }
                            if (currentChar == CR)
                            {
                                bPreviousIsCR = true;
                            }
                            previousChar = currentChar;
                            index++;
                        }
                    }
                    break;

                default:
                    break;
            }
            return bResult;
        }

        public bool ReplaceLineEnding(FilesReplaceInfo filesReplaceInfo, LineEnding lineEndingBefore, LineEnding lineEndingAter)
        {
            bool bResult = false;
            switch (lineEndingBefore)
            {
                case LineEnding.CRLF:
                    filesReplaceInfo.ArrayReplaceIndex.Sort((a, b) => b.CompareTo(a));
                    foreach (int indexReplace in filesReplaceInfo.ArrayReplaceIndex)
                    {
                        bResult = ConvertLineEndingCRLFToNew(filesReplaceInfo.FilePathReplace, lineEndingAter, indexReplace);
                    }
                    break;

                case LineEnding.CR: //CR (\r) = 13
                case LineEnding.LF: //LF (\n) = 10
                    if (lineEndingAter == LineEnding.CRLF)
                    {
                        filesReplaceInfo.ArrayReplaceIndex.Sort((a, b) => b.CompareTo(a));
                        foreach (int indexReplace in filesReplaceInfo.ArrayReplaceIndex)
                        {
                            bResult = ConvertLineEndingToCRLF(filesReplaceInfo.FilePathReplace, lineEndingBefore, indexReplace);
                        }
                    }
                    else
                    {
                        foreach (int indexReplace in filesReplaceInfo.ArrayReplaceIndex)
                        {
                            bResult = ConvertLineEnding(filesReplaceInfo.FilePathReplace, lineEndingAter, indexReplace);
                        }
                    }
                    break;

                default:
                    break;
            }
            return bResult;
        }

        static bool ConvertLineEnding(string filePath, LineEnding lineEndingAter, int replacePosition)
        {
            bool bResult = false;
            try
            {
                string content;

                // Sử dụng StreamReader để đọc từ tệp tin
                using (StreamReader reader = new StreamReader(filePath))
                {
                    content = reader.ReadToEnd();
                }

                // Translate the desired line ending to string
                string translatedLineEnding = TranslateLineEnding(lineEndingAter);

                // Convert the content to a char array
                char[] contentArray = content.ToCharArray();

                // Replace the character at the specified position with the desired line ending
                for (int i = 0; i < translatedLineEnding.Length; i++)
                {
                    contentArray[replacePosition + i] = translatedLineEnding[i];
                }

                // Sử dụng StreamWriter để ghi nội dung mới vào tệp tin
                using (StreamWriter writer = new StreamWriter(filePath, false)) // Mở tệp tin để ghi (overwrite)
                {
                    writer.Write(new string(contentArray));
                    bResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error when performing conversion: {ex.Message}");
            }
            return bResult;
        }
        static bool ConvertLineEndingCRLFToNew(string filePath, LineEnding lineEndingAter, int replacePosition)
        {
            bool bResult = false;
            try
            {
                string content;
                // Sử dụng StreamReader để đọc từ tệp tin
                using (StreamReader reader = new StreamReader(filePath))
                {
                    content = reader.ReadToEnd();
                }

                // Convert the content to a char array
                char[] contentArray = content.ToCharArray();
                int indexToRemove;
                if (lineEndingAter == LineEnding.CR)
                {
                    indexToRemove = replacePosition;
                }
                else
                {
                    indexToRemove = replacePosition - 1;
                }
                int numberOfElementsToRemove = 1;

                // Tạo một mảng mới với các phần tử không bao gồm phần tử thứ 3 và 4
                char[] newArray = new char[contentArray.Length - numberOfElementsToRemove];
                Array.Copy(contentArray, 0, newArray, 0, indexToRemove);
                Array.Copy(contentArray, indexToRemove + numberOfElementsToRemove, newArray, indexToRemove, contentArray.Length - indexToRemove - numberOfElementsToRemove);

                // Sử dụng StreamWriter để ghi nội dung mới vào tệp tin
                using (StreamWriter writer = new StreamWriter(filePath, false)) // Mở tệp tin để ghi (overwrite)
                {
                    writer.Write(new string(newArray));
                    bResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error when performing conversion: {ex.Message}");
            }
            return bResult;
        }

        static bool ConvertLineEndingToCRLF(string filePath, LineEnding lineEndingBefore, int replacePosition)
        {
            bool bResult = false;
            int indexReplaceToCRLF = 0 ;
            char charToAdd;
            if (lineEndingBefore == LineEnding.CR)
            {
                indexReplaceToCRLF = 1;
                charToAdd = '\n';
            }
            else
            { 
                charToAdd = '\r';
            }
            replacePosition = replacePosition + indexReplaceToCRLF;
            try
            {
                string content;
                // Sử dụng StreamReader để đọc từ tệp tin
                using (StreamReader reader = new StreamReader(filePath))
                {
                    content = reader.ReadToEnd();
                }

                // Convert the content to a char array

                char[] contentArray = content.ToCharArray();

                // Tạo mảng mới với ký tự được thêm vào replacePosition
                char[] newArray = new char[contentArray.Length + 1];

                // Sao chép các phần tử trước replacePosition
                Array.Copy(contentArray, 0, newArray, 0, replacePosition);

                // Thêm ký tự vào replacePosition
                newArray[replacePosition] = charToAdd;

                // Sao chép các phần tử sau replacePosition
                Array.Copy(contentArray, replacePosition, newArray, replacePosition + 1, contentArray.Length - replacePosition);


                // Sử dụng StreamWriter để ghi nội dung mới vào tệp tin
                using (StreamWriter writer = new StreamWriter(filePath, false)) // Mở tệp tin để ghi (overwrite)
                {
                    writer.Write(new string(newArray));
                    bResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error when performing conversion: {ex.Message}");
            }
            return bResult;
        }
        static string TranslateLineEnding(LineEnding lineEnding)
        {
            switch (lineEnding)
            {
                case LineEnding.CRLF:
                    return "\r\n"; // Carriage Return + Line Feed (Windows)
                case LineEnding.CR:
                    return "\r";   // Carriage Return (Mac)
                case LineEnding.LF:
                    return "\n";   // Line Feed (Unix/Linux)
                case LineEnding.None:
                default:
                    return string.Empty;
            }
        }

    }
}
