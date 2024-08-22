using AutoMapper;
using Moq;
using UserService.Contracts.Data;
using UserService.Contracts.Repositories;
using UserService.Contracts.Services;
using UserService.Domain;
using UserService.Infrastructure.Services;

namespace UserServiceTests.Services;

[TestClass]
public class AuthServiceTests
{
    private Mock<IMapper> mapperMock;
    private Mock<IUserRepository> userRepositoryMock;
    private Mock<IAuthenticationProviderService> authenticationProviderServiceMock;
    private IAuthService authService;

    public AuthServiceTests()
    {
        mapperMock = new Mock<IMapper>();
        userRepositoryMock = new Mock<IUserRepository>();
        authenticationProviderServiceMock = new Mock<IAuthenticationProviderService>();

        authService = new AuthService(
            mapperMock.Object,
            userRepositoryMock.Object,
            authenticationProviderServiceMock.Object);
    }

    [TestInitialize]
    public void Initialize()
    {
        mapperMock.Reset();
        userRepositoryMock.Reset();
        authenticationProviderServiceMock.Reset();

        authService = new AuthService(
            mapperMock.Object,
            userRepositoryMock.Object,
            authenticationProviderServiceMock.Object);
    }

    [TestMethod]
    public async Task SignupAsync_ValidInputUserExists_ReturnsExistingUser()
    {
        var firebaseId = "firebaseId";

        var userToReturn = new User("firstName", "lastName", "email", "country", "city", "postCode", "street",
            "houseNumber", false)
        {
            Id = Guid.NewGuid()
        };

        var input = new UserSignupInput
        {
            FirebaseId = firebaseId,
            FirstName = "firstName_updated",
            LastName = "lastName_updated",
            Country = "country_updated",
            City = "city_updated",
            PostCode = "postCode_updated",
            Street = "street_updated",
            HouseNumber = "houseNumber_updated",
            IsGuest = true,
        };

        var existingUser = new User
        {
            FirebaseId = input.FirebaseId,
            IsGuest = input.IsGuest
        };

        userRepositoryMock.Setup(x => x.GetByFirebaseIdAsync(firebaseId)).ReturnsAsync(existingUser);

        var createdResult = await authService.SignupAsync(input);

        Assert.AreEqual(existingUser, createdResult);
        userRepositoryMock.Verify(x => x.CreateAsync(userToReturn), Times.Never);
    }

    [TestMethod]
    public async Task SignupAsync_ValidInputUserNotExists_ReturnsCreatedUser()
    {
        var firebaseId = "firebaseId";

        var input = new UserSignupInput
        {
            FirebaseId = firebaseId,
            Email = "email",
            FirstName = "firstName",
            LastName = "lastName",
            Country = "country",
            City = "city",
            PostCode = "postCode",
            Street = "street",
            HouseNumber = "houseNumber",
            IsGuest = true,
        };


        var userToReturn = new User("firstName", "lastName", "email", "country", "city", "postCode", "street",
            "houseNumber", true);

        mapperMock
            .Setup(x => x.Map<User>(input))
            .Returns(userToReturn);

        var createdResult = await authService.SignupAsync(input);

        Assert.AreEqual(input.FirstName, createdResult.FirstName);
        Assert.AreEqual(input.LastName, createdResult.LastName);
        Assert.AreEqual(input.Email, createdResult.Email);
        Assert.AreEqual(input.Country, createdResult.Country);
        Assert.AreEqual(input.City, createdResult.City);
        Assert.AreEqual(input.PostCode, createdResult.PostCode);
        Assert.AreEqual(input.Street, createdResult.Street);
        Assert.AreEqual(input.HouseNumber, createdResult.HouseNumber);
        Assert.AreEqual(input.IsGuest, createdResult.IsGuest);
    }

    [TestMethod]
    public async Task SignupAsync_UserRegisteredAsDifferentRole_ThrowsException()
    {
        var firebaseId = "firebaseId";

        var input = new UserSignupInput
        {
            FirebaseId = firebaseId,
            Email = "email",
            FirstName = "firstName",
            LastName = "lastName",
            Country = "country",
            City = "city",
            PostCode = "postCode",
            Street = "street",
            HouseNumber = "houseNumber",
            IsGuest = true,
        };

        var userToReturn = new User("firstName", "lastName", "email", "country", "city", "postCode", "street",
            "houseNumber", false);

        userRepositoryMock.Setup(x => x.GetByFirebaseIdAsync(firebaseId)).ReturnsAsync(userToReturn);

        await Assert.ThrowsExceptionAsync<Exception>(() => authService.SignupAsync(input));
    }
}
