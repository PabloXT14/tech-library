using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.UseCases.Books.Create;
using TechLibrary.Api.UseCases.Books.Filter;
using TechLibrary.Api.UseCases.Books.Get;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ResponseCreateBookJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] RequestCreateBookJson request)
    {
        var useCase = new CreateBookUseCase();

        var response = useCase.Execute(request);
        
        return Created(string.Empty, response);
    }
    
    [HttpGet("Filter")]
    [ProducesResponseType(typeof(ResponseBooksJson), StatusCodes.Status200OK)]
    public IActionResult Filter(int pageNumber, string? title)
    {
        var useCase = new FilterBookUseCase();

        var request = new RequestFilterBooksJson
        {
            PageNumber = pageNumber,
            Title = title
        };
        
        var result = useCase.Execute(request);
        
        return Ok(result);
    }

    [HttpGet("{bookId}")]
    [ProducesResponseType(typeof(ResponseBookJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    public IActionResult Get([FromRoute] Guid bookId)
    {
        var useCase = new GetBookUseCase();
        
        var response = useCase.Execute(bookId);
        
        return Ok(response);
    }
}
