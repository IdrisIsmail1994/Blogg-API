using StudentBloggAPI.Mappers;
using StudentBloggAPI.Mappers.Interfaces;
using StudentBloggAPI.Models.DTOs;
using StudentBloggAPI.Models.Entities;
using StudentBloggAPI.Repository.Interfaces;
using StudentBloggAPI.Services.Interfaces;

namespace StudentBloggAPI.Services
{
	public class PostService : IPostService
	{
		private readonly IPostRepository _postRepository;
		private readonly IMapper<Post, PostDTO> _postMapper;

		public PostService(IPostRepository postRepository, IMapper<Post, PostDTO> postMapper)
		{
			_postRepository = postRepository;
			_postMapper = postMapper;
		}

		public async Task<PostDTO?> AddPostAsync(PostDTO postDTO)
		{
			var postToAdd = _postMapper.MapToModel(postDTO);

			var addedPost = await _postRepository.AddPostAsync(postToAdd);

			return addedPost != null ? _postMapper.MapToDTO(addedPost) : null;
		}

		public async Task<PostDTO?> DeletePostAsync(int deletePostId, int loginUserId)
		{
			var deletedPost = await _postRepository.DeletePostByIdAsync(deletePostId);
			return deletedPost != null ? _postMapper.MapToDTO(deletedPost) : null;
		}

		public async Task<IEnumerable<PostDTO>> GetAllPostsAsync()
		{
			var posts = await _postRepository.GetPostsAsync();
			return posts.Select(post => _postMapper.MapToDTO(post)).ToList();
		}

		public async Task<PostDTO?> GetPostByIdAsync(int id)
		{
			var post = await _postRepository.GetPostByIdAsync(id);
			return post != null ? _postMapper.MapToDTO(post) : null;
		}

		public async Task<PostDTO?> UpdatePostAsync(int id, PostDTO postDTO)
		{
			var post = _postMapper.MapToModel(postDTO);
			var updatedPost = await _postRepository.UpdatePostAsync(id, post);
			return updatedPost != null ? _postMapper.MapToDTO(updatedPost) : null;
		}
	}
}
