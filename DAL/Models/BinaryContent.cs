namespace Hubler.DAL.Models;

public class BinaryContent
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string FileType { get; set; }
    public string FileExtension { get; set; }
    public byte[] Content { get; set; }
    public DateTime UploadDate { get; set; }
}