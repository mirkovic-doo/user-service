namespace UserService.Controllers._Common.Request;

public record UserRequest
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Country { get; set; }
    public required string City { get; set; }
    public required string PostCode { get; set; }
    public required string Street { get; set; }
    public required string HouseNumber { get; set; }
}
