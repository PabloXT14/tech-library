using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.Services.LoggedUser;
using TechLibrary.Api.UseCases.Users.Get;
using TechLibrary.Api.UseCases.Users.Register;
using TechLibrary.Api.UseCases.Users.Update;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
    public IActionResult Register(RequestUserJson request)
    {
        var useCase = new RegisterUserUseCase();

        var response = useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ResponseUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Get()
    {
        var loggedUser = new LoggedUserService(HttpContext);

        var useCase = new GetUserUseCase(loggedUser);

        var response = useCase.Execute();

        return Ok(response);
    }
    
    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
    public IActionResult Update(RequestUpdateUserJson request)
    {
        var loggedUser = new LoggedUserService(HttpContext);

        var useCase = new UpdateUserUseCase(loggedUser);

        useCase.Execute(request);
        
        return NoContent();
    }
}

