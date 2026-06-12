using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.Services.LoggedUser;
using TechLibrary.Api.UseCases.Checkouts.Delete;
using TechLibrary.Api.UseCases.Checkouts.Extend;
using TechLibrary.Api.UseCases.Checkouts.Register;
using TechLibrary.Api.UseCases.Checkouts.Return;
using TechLibrary.Api.UseCases.Checkouts.Update;
using TechLibrary.Communication.Requests;
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
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status409Conflict)]
    public IActionResult BookCheckout(Guid bookId)
    {
        var loggedUser = new LoggedUserService(HttpContext);

        var useCase = new RegisterBookCheckoutUseCase(loggedUser);

        useCase.Execute(bookId);

        return NoContent();
    }

    [HttpPatch]
    [Route("{bookId}/return")]
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

    [HttpPatch]
    [Route("{checkoutId}/update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    public IActionResult UpdateBook(Guid checkoutId, [FromBody] RequestUpdateCheckoutJson request)
    {
        var loggedUser = new LoggedUserService(HttpContext);

        var useCase = new UpdateCheckoutUseCase(loggedUser);

        useCase.Execute(checkoutId, request);

        return NoContent();
    }

    [HttpPatch]
    [Route("{checkoutId}/extend")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status409Conflict)]
    public IActionResult ExtendCheckout(Guid checkoutId)
    {
        var loggedUser = new LoggedUserService(HttpContext);

        var useCase = new ExtendCheckoutUseCase(loggedUser);

        useCase.Execute(checkoutId);

        return NoContent();
    }

    [HttpDelete]
    [Route("{checkoutId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status409Conflict)]
    public IActionResult DeleteCheckout(Guid checkoutId)
    {
        var useCase = new DeleteCheckoutUseCase();

        useCase.Execute(checkoutId);

        return NoContent();
    }
}

