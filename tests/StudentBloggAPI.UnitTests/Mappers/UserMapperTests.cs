using StudentBloggAPI.Mappers;
using StudentBloggAPI.Mappers.Interfaces;
using StudentBloggAPI.Models.DTOs;
using StudentBloggAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentBloggAPI.UnitTests.Mappers;

public class UserMapperTests
{
    private readonly IMapper<User, UserDTO> _userMapper = new UserMapper();

    [Fact]
    public void MapToDTO_When_UserEntity_Given_ShouldReturn_UserDTO()
    {
        // Arrange
        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        User user = new User
        {
            Created = new DateTime(2023, 11, 7, 12, 0, 0),
            Email = "ola@gmail.com",
            FirstName = "Ola",
            LastName = "Normann",
            IsAdminUser = false,
            Id = 1,
            Updated = new DateTime(2023, 11,7,12, 30, 0),
            UserName = "Ola",
            Salt = salt,
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("hemmelig", salt)
        };

        // Act 
        var userDTO = _userMapper.MapToDTO(user);

        // Assert
        Assert.NotNull(userDTO);
        Assert.Equal(userDTO.FirstName, user.FirstName);
        Assert.Equal(userDTO.LastName, user.LastName);
        Assert.Equal(userDTO.UserName, user.UserName);
        Assert.Equal(userDTO.Email, user.Email);
        Assert.Equal(userDTO.Id, user.Id);
        Assert.Equal(userDTO.Created, user.Created);

    }

}
