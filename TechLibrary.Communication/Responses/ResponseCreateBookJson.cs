namespace TechLibrary.Communication.Responses;

public class ResponseCreateBookJson
{
    public Guid BookId { get; set; } = Guid.NewGuid();
}