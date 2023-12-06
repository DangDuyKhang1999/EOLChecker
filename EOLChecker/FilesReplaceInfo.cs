public class FilesReplaceInfo
{
    public string FilePathReplace { get; set; }

    public List<int> ArrayReplaceIndex { get; set; }

    public FilesReplaceInfo(string filePathReplace,  List<int> arrayNumbers)
    {
        FilePathReplace = filePathReplace;
        ArrayReplaceIndex = arrayNumbers;
    }
}
