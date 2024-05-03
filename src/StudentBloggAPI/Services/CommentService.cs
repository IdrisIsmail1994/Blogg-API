using Microsoft.AspNetCore.Http.HttpResults;
using StudentBloggAPI.Mappers.Interfaces;
using StudentBloggAPI.Models.DTOs;
using StudentBloggAPI.Models.Entities;
using StudentBloggAPI.Repository.Interfaces;
using StudentBloggAPI.Services.Interfaces;

namespace StudentBloggAPI.Services
{
	public class CommentService : ICommentService
	{
		private readonly ICommentRepository _commentRepository;
		private readonly IMapper<Comment, CommentDTO> _commentMapper;
		private readonly ILogger<CommentService> _logger;

		public CommentService(ICommentRepository commentService, IMapper<Comment, CommentDTO> mapperService, ILogger<CommentService> logger)
		{
			_commentRepository = commentService;
			_commentMapper = mapperService;
			_logger = logger;
		}

		public async Task<CommentDTO?> AddCommentAsync(int commentId, CommentDTO commentDTO, int inloggedUserId)
		{
			var comment = _commentMapper.MapToModel(commentDTO);
			comment.PostId = commentId;

			var addedComent = await _commentRepository.AddCommentAsync(comment);

			if (addedComent != null)
			{
				return _commentMapper.MapToDTO(addedComent);
			}
			else
			{
				_logger?.LogError("Feil ved forsøk å legge til en kommentar - added comment er null");
				return null; 
			}
		}


		public async Task<CommentDTO?> DeleteCommentAsync(int commentId, int inloggedUserId)
		{
			try
			{
				var deletedComment = await _commentRepository.DeleteCommentAsync(commentId, inloggedUserId);
				if (deletedComment == null) 
				{
					_logger?.LogWarning("Feil ved sletting av kommentar med ID: {@comment} ", commentId);
					return null;
				}
				return _commentMapper.MapToDTO(deletedComment);
			}
			catch (Exception ex)
			{
				_logger?.LogError("Feil ved forsøk på å slette kommentar med ID: {@comment}", commentId);
				throw;
			}
		}

		public async Task<IEnumerable<CommentDTO?>> GetAllCommentsAsync(int pageNr, int pageSize)
		{
			try
			{
				var comments = await _commentRepository.GetAllCommentsAsync(pageNr, pageSize);
				return comments.Select(comment => _commentMapper.MapToDTO(comment));
			}
			catch (Exception ex)
			{
				_logger?.LogError("Feil ved forsøk på å hente alle kommentarer");
				throw;
			}

		}

		public async Task<CommentDTO?> GetCommentByIdAsync(int id)
		{
			try
			{
				var comment = await _commentRepository.GetCommentByIdAsync(id);

				if (comment == null)
				{
					_logger?.LogWarning("Kommentar med ID {CommentId} ble ikke funnet", id);
					return null;
				}

				return _commentMapper.MapToDTO(comment);
			}
			catch (Exception ex)
			{
				_logger?.LogError(ex, "Feil ved forsøk på å hente kommentar med ID {CommentId}", id);
				throw;
			}
		}

		public async Task<CommentDTO?> UpdateCommentAsync(int commentId, CommentDTO commentDTO, int inloggedUserId)
		{
			try
			{
				var existingComment = await _commentRepository.GetCommentByIdAsync(commentId);
				if(existingComment == null)
				{
					_logger?.LogError("Forøkte å oppdate en ikke-eksisterende kommentar {@commentID}.", commentId);
					return null;
				}

				existingComment.Content = commentDTO.Content;
				existingComment.Updates = DateTime.Now;

				var updatedComment = await _commentRepository.UpdateCommentAsync(commentId, existingComment);
				return _commentMapper.MapToDTO(updatedComment);
			}
			catch (Exception ex)
			{
				_logger?.LogError(ex, "Feil ved forsøk på å oppdatere kommentar med ID {@commentID}", commentId);

				throw;
			}
		}

	}
}