using Microsoft.EntityFrameworkCore;
using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.UseCases.Books.Filter;

public class FilterBookUseCase
{
    private const int PAGE_SIZE = 10;
    
    public ResponseBooksJson Execute(RequestFilterBooksJson request)
    {
        var dbContext = new TechLibraryDbContext();
        
        var skip  = (request.PageNumber - 1) * PAGE_SIZE;

        var query = dbContext.Books.AsQueryable(); // Inicia a query com todos os books do banco de dados

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            query = query.Where(book => EF.Functions.Like(book.Title, $"%{request.Title}%")); // Adiciona um filtro para o título, usando o operador LIKE do SQL
        }
        
        var books = query
            .OrderBy(book => book.Title)
            .ThenBy(book => book.Author)
            .Skip(skip)
            .Take(PAGE_SIZE)
            .ToList(); // Este comando é que será o responsável por executar a query no banco de dados

        var totalCount = 0;

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            totalCount = dbContext.Books.Count();
        }
        else
        {
            totalCount = dbContext.Books.Count(book => EF.Functions.Like(book.Title, $"%{request.Title}%"));
        }
        
        return new ResponseBooksJson
        {
            Books = books.Select(book => new ResponseBookJson
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Amount = book.Amount
            }).ToList(),
            Pagination = new ResponsePaginationJson
            {
                PageNumber = request.PageNumber,
                TotalCount = totalCount,
            }
        };
    }
}