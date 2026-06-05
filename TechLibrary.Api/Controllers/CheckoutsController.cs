using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.Services.LoggedUser;
using TechLibrary.Api.UseCases.Checkouts;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class CheckoutsController : ControllerBase
{
    [HttpPost]
    [Route("{bookId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult BookCheckout(Guid bookId)
    {
        var loggedUser = new LoggedUserService(HttpContext);
        
        var useCase = new RegisterBookCheckoutUseCase(loggedUser);
        
        useCase.Execute(bookId);
        
        return NoContent();
    }
    
    [HttpPatch]
    [Route("{bookId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status409Conflict)]
    public IActionResult ReturnBook(Guid bookId)
    {
        var loggedUser = new LoggedUserService(HttpContext);
        
        var useCase = new ReturnBookUseCase(loggedUser);
        
        useCase.Execute(bookId);
        
        return NoContent();
    }
}

