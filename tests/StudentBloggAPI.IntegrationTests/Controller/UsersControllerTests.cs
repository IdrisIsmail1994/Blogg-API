using Moq;
using Newtonsoft.Json;
using StudentBloggAPI.IntegrationTests.Controller.TestData;
using StudentBloggAPI.Models.DTOs;
using StudentBloggAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StudentBloggAPI.IntegrationTests.Controller;

public class UsersControllerTests : IDisposable
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public UsersControllerTests()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _factory.Dispose();
        _client.Dispose();
    }

    [Fact]
    public async Task GetUsersAsync_DefaultPageSize_ReturnTwoUser()
    {
        // Arrange
        List<User> users = new();
        User userA = new User
        {
            Created = new DateTime(2023, 11, 14, 9, 30, 0),
            Email = "ola@gmail.com",
            FirstName = "Ola",
            UserName = "ola",
            LastName = "Normann",
            Id = 1,
            IsAdminUser = false,
            Salt = "$2a$13$HkriD/2KXmhHmfBJIM5cFO",
            HashedPassword = "$2a$13$HkriD/2KXmhHmfBJIM5cFOVCQvSigHBCeWk1G2Qv068u6oSf/v52G",
            Updated = new DateTime(2023, 11, 14, 9, 30, 0)
        };
        User userB = new User
        {
            Created = new DateTime(2023, 11, 14, 9, 30, 0),
            Email = "kari@gmail.com",
            FirstName = "Kari",
            UserName = "kari",
            LastName = "Normann",
            Id = 2,
            IsAdminUser = false,
            Salt = "$2a$13$qOtZSfQ4FWjn.zBUEhukV.",
            HashedPassword = "$2a$13$qOtZSfQ4FWjn.zBUEhukV.4wKuxw4VwrpQMs4inUg/I7DMwdcyEQm",
            Updated = new DateTime(2023, 11, 14, 9, 30, 0)
        };
        users.Add(userA);
        users.Add(userB);

        _factory.UserRespostoryMock.Setup(u => u.GetPageAsync(1, 10)).ReturnsAsync(users);
        _factory.UserRespostoryMock.Setup(u => u.GetUserByNameAsync(userA.UserName)).ReturnsAsync(userA);

        // login for 'ola'
        string base64EncodedAuthenticationString = "b2xhOk8xYW5vcm1hbm4j";
        _client.DefaultRequestHeaders.Add("Authorization", $"Basic {base64EncodedAuthenticationString}");


        // Act
        var response = await _client.GetAsync("/api/v1/Users");

        // model binding
        var data = JsonConvert
            .DeserializeObject<IEnumerable<UserDTO>>(await response.Content.ReadAsStringAsync());


        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(data);
        Assert.Collection(data, 
                    u =>
                    {
                        Assert.Equal(userA.FirstName, u.FirstName);
                        Assert.Equal(userA.LastName, u.LastName);
                        Assert.Equal(userA.Email, u.Email);
                        Assert.Equal(userA.UserName, u.UserName);
                        Assert.Equal(userA.Id, u.Id);
                        Assert.Equal(userA.Created, u.Created);
                    },
                    u =>
                    {
                        Assert.Equal(userB.FirstName, u.FirstName);
                        Assert.Equal(userB.LastName, u.LastName);
                        Assert.Equal(userB.Email, u.Email);
                        Assert.Equal(userB.UserName, u.UserName);
                        Assert.Equal(userB.Id, u.Id);
                        Assert.Equal(userB.Created, u.Created);
                    });

    }

    [Theory]
    [MemberData(nameof(TestUserDataItems.GetTestUsers), MemberType = typeof(TestUserDataItems))]
    public async Task GetUsersAsync_DefaultPageSize_ReturnOneUser(TestUser testUser)
    {
        // Arrange
        User user = testUser.User!;

        _factory.UserRespostoryMock
            .Setup(u => u.GetPageAsync(1, 10))
            .ReturnsAsync(new List<User> { user! });

        _factory.UserRespostoryMock
            .Setup(u => u.GetUserByNameAsync(user.UserName))
            .ReturnsAsync(user);

        _client.DefaultRequestHeaders.Add("Authorization", $"Basic {testUser.Base64EncodedUsernamePassword}");


        // Act
        var response = await _client.GetAsync("/api/v1/Users");

        var data = JsonConvert.DeserializeObject<IEnumerable<UserDTO>>(await response.Content.ReadAsStringAsync());

        // Assert
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(data);
        Assert.Collection(data,
            response_user =>
            {
                Assert.Equal(user.FirstName, response_user.FirstName);
                Assert.Equal(user.LastName, response_user.LastName);
                Assert.Equal(user.Email, response_user.Email);
                Assert.Equal(user.Created, response_user.Created);
                Assert.Equal(user.UserName, response_user.UserName);
                Assert.Equal(user.Id, response_user.Id);
            });

    }

    //[Fact]
    //public async Task GetUserByIdAsync_WhenIdIsGiven_ShouldReturn_UserDTOWithId()
    //{
    //    // Arrange
    //    //_factory.UserRespostoryMock.Setup(x => x.GetUserB)

    //    // Act
    //    _client.DefaultRequestHeaders.Add("Authorization", "Basic eW5ndmU6aGVtbWVsaWc=");
    //    var response = await _client.GetAsync("/api/v1/Users/2");

    //    var data = JsonConvert.DeserializeObject<UserDTO>(await response.Content.ReadAsStringAsync());

    //    // Assert
    //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    //    Assert.NotNull(data);
    //    Assert.Equal("Per", data.FirstName);
    //}
}
