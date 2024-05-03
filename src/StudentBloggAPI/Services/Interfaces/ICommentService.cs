using StudentBloggAPI.Models.DTOs;

namespace StudentBloggAPI.Services.Interfaces
{
	public interface ICommentService
	{
		Task<IEnumerable<CommentDTO?>> GetAllCommentsAsync(int pageNr, int pageSize);
		Task<CommentDTO?> GetCommentByIdAsync(int commentid);

		Task<CommentDTO?> AddCommentAsync(int commentId, CommentDTO commentDTO, int inloggedUserId);
		Task<CommentDTO?> UpdateCommentAsync(int commentId, CommentDTO commentDTO, int inloggedUserId);
		Task<CommentDTO?> DeleteCommentAsync(int commentId, int inloggedUserId);

	}
}
