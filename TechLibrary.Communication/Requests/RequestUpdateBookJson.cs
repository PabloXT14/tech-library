namespace TechLibrary.Communication.Requests;

public class RequestUpdateBookJson
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public int? Amount { get; set; }
}