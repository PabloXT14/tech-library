namespace TechLibrary.Api.Domain.Entities;

public class Checkout
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BookId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CheckoutDate { get; set; } = DateTime.UtcNow;
    public DateTime ExpectedReturnDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
}