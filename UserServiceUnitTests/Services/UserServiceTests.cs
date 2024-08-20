using AutoMapper;
using Moq;
using UserService.Contracts.Data;
using UserService.Contracts.Providers;
using UserService.Contracts.Repositories;
using UserService.Contracts.Services;
using UserService.Domain;
using UserServiceImplementation = UserService.Infrastructure.Services.UserService;

namespace UserServiceTests.Services;

[TestClass]
public class UserServiceTests
{
    private Mock<IMapper> mapperMock;
    private Mock<IUserProvider> userProviderMock;
    private Mock<IUserRepository> userRepositoryMock;
    private Mock<IAuthenticationProviderService> authenticationProviderServiceMock;
    private IUserService userService;

    public UserServiceTests()
    {
        mapperMock = new Mock<IMapper>();
        userProviderMock = new Mock<IUserProvider>();
        userRepositoryMock = new Mock<IUserRepository>();
        authenticationProviderServiceMock = new Mock<IAuthenticationProviderService>();

        userService = new UserServiceImplementation(mapperMock.Object, userProviderMock.Object, userRepositoryMock.Object, authenticationProviderServiceMock.Object);
    }

    [TestInitialize]
    public void Setup()
    {
        mapperMock = new Mock<IMapper>();
        userProviderMock = new Mock<IUserProvider>();
        userRepositoryMock = new Mock<IUserRepository>();
        authenticationProviderServiceMock = new Mock<IAuthenticationProviderService>();

        userService = new UserServiceImplementation(mapperMock.Object, userProviderMock.Object, userRepositoryMock.Object, authenticationProviderServiceMock.Object);
    }

    [TestMethod]
    public async Task GetMeAsync_UserProviderValidReturn_ReturnsUser()
    {
        var userToReturn = new User("username", "firstName", "lastName", "email", "country", "city", "postCode", "street", "houseNumber", false);

        userProviderMock
            .Setup(x => x.GetMeAsync())
            .ReturnsAsync(userToReturn);

        var getMeResult = await userService.GetMeAsync();

        Assert.AreEqual(userToReturn.Username, getMeResult.Username);
        Assert.AreEqual(userToReturn.FirstName, getMeResult.FirstName);
        Assert.AreEqual(userToReturn.LastName, getMeResult.LastName);
        Assert.AreEqual(userToReturn.Email, getMeResult.Email);
        Assert.AreEqual(userToReturn.Country, getMeResult.Country);
        Assert.AreEqual(userToReturn.City, getMeResult.City);
        Assert.AreEqual(userToReturn.PostCode, getMeResult.PostCode);
        Assert.AreEqual(userToReturn.Street, getMeResult.Street);
        Assert.AreEqual(userToReturn.HouseNumber, getMeResult.HouseNumber);
        Assert.AreEqual(userToReturn.IsGuest, getMeResult.IsGuest);
    }

    [TestMethod]
    public async Task GetMeAsync_UserProviderThrowsException_ThrosException()
    {
        userProviderMock
            .Setup(x => x.GetMeAsync())
            .ThrowsAsync(new Exception("User not found"));

        await Assert.ThrowsExceptionAsync<Exception>(() => userService.GetMeAsync());
    }

    [TestMethod]
    public async Task DeleteAsync_Valid_CallsMethodsOnce()
    {
        var userToReturn = new User("username", "firstName", "lastName", "email", "country", "city", "postCode", "street", "houseNumber", false);

        userProviderMock
            .Setup(x => x.GetMeAsync())
            .ReturnsAsync(userToReturn);

        await userService.DeleteAsync();

        authenticationProviderServiceMock.Verify(x => x.DeleteProvidedUserAsync(userToReturn), Times.Once);
        userRepositoryMock.Verify(x => x.DeleteAsync(userToReturn), Times.Once);
    }

    [TestMethod]
    public async Task UpdateAsync_ValidInput_ReturnsUpdatedUser()
    {
        var userInput = new UserInput
        {
            Username = "username_updated",
            FirstName = "firstName_updated",
            LastName = "lastName_updated",
            Country = "country_updated",
            City = "city_updated",
            PostCode = "postCode_updated",
            Street = "street_updated",
            HouseNumber = "houseNumber_updated"
        };

        var userToReturn = new User("username", "firstName", "lastName", "email", "country", "city", "postCode", "street", "houseNumber", false);

        userProviderMock
            .Setup(x => x.GetMeAsync())
            .ReturnsAsync(userToReturn);

        mapperMock
            .Setup(x => x.Map(userInput, userToReturn))
            .Callback((UserInput input, User user) =>
            {
                user.Username = input.Username;
                user.FirstName = input.FirstName;
                user.LastName = input.LastName;
                user.Country = input.Country;
                user.City = input.City;
                user.PostCode = input.PostCode;
                user.Street = input.Street;
                user.HouseNumber = input.HouseNumber;
                user.IsGuest = false;
            });

        var updateResult = await userService.UpdateAsync(userInput);

        Assert.AreEqual(userInput.Username, updateResult.Username);
        Assert.AreEqual(userInput.FirstName, updateResult.FirstName);
        Assert.AreEqual(userInput.LastName, updateResult.LastName);
        Assert.AreEqual(userToReturn.Email, updateResult.Email);
        Assert.AreEqual(userInput.Country, updateResult.Country);
        Assert.AreEqual(userInput.City, updateResult.City);
        Assert.AreEqual(userInput.PostCode, updateResult.PostCode);
        Assert.AreEqual(userInput.Street, updateResult.Street);
        Assert.AreEqual(userInput.HouseNumber, updateResult.HouseNumber);
        Assert.AreEqual(userToReturn.IsGuest, updateResult.IsGuest);
    }

    [TestMethod]
    public async Task UpdateAsync_UsernameAlreadyExists_ThrowsException()
    {
        var userInput = new UserInput
        {
            Username = "username_updated",
            FirstName = "firstName_updated",
            LastName = "lastName_updated",
            Country = "country_updated",
            City = "city_updated",
            PostCode = "postCode_updated",
            Street = "street_updated",
            HouseNumber = "houseNumber_updated"
        };

        var userToReturn = new User("username", "firstName", "lastName", "email", "country", "city", "postCode", "street", "houseNumber", false)
        {
            Id = Guid.NewGuid()
        };

        var userWithUsername = new User("username_updated", "firstName", "lastName", "email", "country", "city", "postCode", "street", "houseNumber", false)
        {
            Id = Guid.NewGuid()
        };

        userProviderMock
            .Setup(x => x.GetMeAsync())
            .ReturnsAsync(userToReturn);

        userRepositoryMock
            .Setup(x => x.GetByUsernameAsync(userInput.Username))
            .ReturnsAsync(userWithUsername);

        await Assert.ThrowsExceptionAsync<Exception>(() => userService.UpdateAsync(userInput));
    }

    [TestMethod]
    public async Task UpdateAsync_ValidInputSameUsernameSameUser_ReturnsUpdatedUser()
    {
        var sameId = Guid.NewGuid();

        var userInput = new UserInput
        {
            Username = "username_updated",
            FirstName = "firstName_updated",
            LastName = "lastName_updated",
            Country = "country_updated",
            City = "city_updated",
            PostCode = "postCode_updated",
            Street = "street_updated",
            HouseNumber = "houseNumber_updated"
        };

        var userToReturn = new User("username", "firstName", "lastName", "email", "country", "city", "postCode", "street", "houseNumber", false)
        {
            Id = sameId
        };

        var userWithUsername = new User("username_updated", "firstName", "lastName", "email", "country", "city", "postCode", "street", "houseNumber", false)
        {
            Id = sameId
        };

        userProviderMock
            .Setup(x => x.GetMeAsync())
            .ReturnsAsync(userToReturn);

        userRepositoryMock
            .Setup(x => x.GetByUsernameAsync(userInput.Username))
            .ReturnsAsync(userWithUsername);

        mapperMock
         .Setup(x => x.Map(userInput, userToReturn))
         .Callback((UserInput input, User user) =>
         {
             user.Username = input.Username;
             user.FirstName = input.FirstName;
             user.LastName = input.LastName;
             user.Country = input.Country;
             user.City = input.City;
             user.PostCode = input.PostCode;
             user.Street = input.Street;
             user.HouseNumber = input.HouseNumber;
             user.IsGuest = false;
         });

        var updateResult = await userService.UpdateAsync(userInput);

        Assert.AreEqual(userInput.Username, updateResult.Username);
        Assert.AreEqual(userInput.FirstName, updateResult.FirstName);
        Assert.AreEqual(userInput.LastName, updateResult.LastName);
        Assert.AreEqual(userToReturn.Email, updateResult.Email);
        Assert.AreEqual(userInput.Country, updateResult.Country);
        Assert.AreEqual(userInput.City, updateResult.City);
        Assert.AreEqual(userInput.PostCode, updateResult.PostCode);
        Assert.AreEqual(userInput.Street, updateResult.Street);
        Assert.AreEqual(userInput.HouseNumber, updateResult.HouseNumber);
        Assert.AreEqual(userToReturn.IsGuest, updateResult.IsGuest);
    }

    [TestMethod]
    public async Task GetUserEmailByUsername_ExistingUsername_ReturnsEmail()
    {
        var userToReturn = new User("username", "firstName", "lastName", "email", "country", "city", "postCode", "street", "houseNumber", false);

        userRepositoryMock
            .Setup(x => x.GetByUsernameAsync(userToReturn.Username))
            .ReturnsAsync(userToReturn);

        var email = await userService.GetUserEmailByUsernameAsync(userToReturn.Username);

        Assert.AreEqual(userToReturn.Email, email);
    }

    [TestMethod]
    public async Task GetUserEmailByUsername_NonExistingUsername_ReturnsEmptyString()
    {
        var email = await userService.GetUserEmailByUsernameAsync("username");

        Assert.AreEqual(string.Empty, email);
    }
}
