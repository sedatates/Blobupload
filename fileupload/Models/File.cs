namespace fileupload.Models;

// File.cs
public class File
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string FileType { get; set; }
    public DateTime UploadDate { get; set; }
    // Diğer özellikler...
}