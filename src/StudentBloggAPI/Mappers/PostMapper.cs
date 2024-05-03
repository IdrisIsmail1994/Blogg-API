using StudentBloggAPI.Mappers.Interfaces;
using StudentBloggAPI.Models.DTOs;
using StudentBloggAPI.Models.Entities;

namespace StudentBloggAPI.Mappers
{
	public class PostMapper : IMapper<Post, PostDTO>
	{
		public PostDTO MapToDTO(Post model)
		{
			return new PostDTO(model.Id, model.UserId, model.Title, model.Content, model.Created);
		}

		public Post MapToModel(PostDTO dto)
		{
			return new Post();
		}
	}
}
