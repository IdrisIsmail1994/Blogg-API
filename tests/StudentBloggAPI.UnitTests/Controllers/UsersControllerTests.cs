using Microsoft.AspNetCore.Mvc;
using Moq;
using StudentBloggAPI.Controllers;
using StudentBloggAPI.Models.DTOs;
using StudentBloggAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentBloggAPI.UnitTests.Controllers;

public class UsersControllerTests
{
    private readonly UsersController _userController;
    private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();

    public UsersControllerTests()
    {
        _userController = new UsersController(_userServiceMock.Object);
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenIdIsGiven_ShouldReturn_UserDTOWithId()
    {
        // Arrange
        int id = 1;
        var userDTO = new UserDTO(id, 
            "ola", "Ola", "Normann",
            "ola@gmail.com", new DateTime(2023, 11, 7, 12, 0, 0));


        _userServiceMock.Setup(x => x.GetUserByIdAsync(id))
            .ReturnsAsync(userDTO);

        // Act
        var res = await _userController.GetUserByIdAsync(id);


        // Assert
        var actionResult = Assert.IsType<ActionResult<UserDTO>>(res);
        var returnValue = Assert.IsType<OkObjectResult>(actionResult.Result);
        var dto = Assert.IsType<UserDTO>(returnValue.Value);

        Assert.Equal(dto.UserName, userDTO.UserName);
        Assert.Equal(dto.FirstName, userDTO.FirstName);
        Assert.Equal(dto.LastName, userDTO.LastName);
        Assert.Equal(dto.Email, userDTO.Email);
        Assert.Equal(dto.Id, userDTO.Id);
        Assert.Equal(dto.Created, userDTO.Created);

    }

    [Fact]
    public async Task GetUserByIdAsync_WhenIdIsGivenAndNotFound_ShouldReturn_NotFound()
    {
        // Arrange
        int id = 1000;

        _userServiceMock.Setup(x => x.GetUserByIdAsync(id))
            .ReturnsAsync(() => null);

        // Act
        var res = await _userController.GetUserByIdAsync(id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<UserDTO>>(res);
        var returnValue = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        var errorMessage = Assert.IsType<string>(returnValue.Value);
        Assert.Equal("Fant ikke bruker", errorMessage);
    }

    [Fact]
    public async Task GetUsers_ShouldReturn_UserDTOs()
    {
        // Arrange
        List<UserDTO> dtos = new() {
            new UserDTO(1,"ola", "Ola", "Normann","ola@gmail.com", new DateTime(2023, 11, 7, 12, 0, 0)),
            new UserDTO(2,"kari", "Kari", "Normann","kar@gmail.com", new DateTime(2023, 11, 7, 12, 0, 0))
        };

        _userServiceMock.Setup(x => x.GetPageAsync(1, 10)).ReturnsAsync(dtos);

        // Act
        var result = await _userController.GetUsersAsync();

        // Assert 
        var actionResult = Assert.IsType<ActionResult<IEnumerable<UserDTO>>>(result);
        var returnValue = Assert.IsType<OkObjectResult>(actionResult.Result);
        var dtos_result = Assert.IsType<List<UserDTO>>(returnValue.Value);
        Assert.Equal(2, dtos.Count());

        var dto = dtos.FirstOrDefault();
        Assert.Equal("ola", dto?.UserName);
    }
}
