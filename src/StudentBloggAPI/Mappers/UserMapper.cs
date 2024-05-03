using StudentBloggAPI.Mappers.Interfaces;
using StudentBloggAPI.Models.DTOs;
using StudentBloggAPI.Models.Entities;

namespace StudentBloggAPI.Mappers
{
	public class UserMapper : IMapper<User, UserDTO>
	{
		public UserDTO MapToDTO(User model)
		{
			return new UserDTO(model.Id, model.UserName, model.FirstName,
				model.LastName, model.Email, model.Created);
		}

		public User MapToModel(UserDTO dto)
		{
			return new User()
			{
				//Id = dto.Id,
				//UserName = dto.UserName,
				//FirstName = dto.FirstName,
				//LastName = dto.LastName,
				//Email = dto.Email,
				//Created = dto.Created
			};
		}
	}
}
