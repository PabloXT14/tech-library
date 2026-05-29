namespace TechLibrary.Communication.Requests;

public class RequestUpdateUserJson
{
    public string? Name { get; set; }
    
    public string? Email { get; set; }
    
    public string? OldPassword { get; set; }
    
    public string? NewPassword { get; set; }
}