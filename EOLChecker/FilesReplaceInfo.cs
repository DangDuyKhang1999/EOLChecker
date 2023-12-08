public class FilesReplaceInfo
{
    public string FilePathReplace { get; set; }

    public List<int> ArrayReplaceIndex { get; set; }

    public List<int> ArrayLineCode { get; set; }

    public FilesReplaceInfo(string filePathReplace,  List<int> arrayNumbers, List<int> arrayLineCode)
    {
        FilePathReplace = filePathReplace;
        ArrayReplaceIndex = arrayNumbers;
        ArrayLineCode = arrayLineCode;
    }
}
