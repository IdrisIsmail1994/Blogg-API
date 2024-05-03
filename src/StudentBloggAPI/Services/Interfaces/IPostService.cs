using StudentBloggAPI.Models.DTOs;

namespace StudentBloggAPI.Services.Interfaces;

public interface IPostService
{
	// CRUD: create - read - update - delete

	//Read
	Task<IEnumerable<PostDTO>> GetAllPostsAsync();

	Task<PostDTO?> GetPostByIdAsync(int id);

	//Create
	Task<PostDTO?> AddPostAsync(PostDTO PostDTO);

	//Update
	Task<PostDTO?> UpdatePostAsync(int id, PostDTO postDTO);

	//Delete
	Task<PostDTO?> DeletePostAsync(int id, int loginUserId);
}
