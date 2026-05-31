using System.Net;

namespace TechLibrary.Exception;

public class NotActiveException : TechLibraryException
{
    public NotActiveException() : base("Usuário inativo ou desativado.") { }
    
    public override List<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Forbidden;
}