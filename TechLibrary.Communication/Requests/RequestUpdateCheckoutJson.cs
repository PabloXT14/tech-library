namespace TechLibrary.Communication.Requests;

public class RequestUpdateCheckoutJson
{
    public DateTime? ExpectedReturnDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
}