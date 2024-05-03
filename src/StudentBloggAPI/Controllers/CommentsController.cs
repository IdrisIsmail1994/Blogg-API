using Microsoft.AspNetCore.Mvc;
using StudentBloggAPI.Models.DTOs;
using StudentBloggAPI.Services.Interfaces;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentBloggAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CommentsController : ControllerBase
	{
		private readonly ICommentService _commentService;

		public CommentsController(ICommentService commentService)
		{
			_commentService = commentService;
		}

		// GET: api/<CommentsCotroller>
		[HttpGet(Name = "GetAllComments")]
		public async Task<ActionResult<IEnumerable<CommentDTO>>> GetComments(int pageNr = 1, int pageSize = 10)
		{
			try
			{
				var comments = await _commentService.GetAllCommentsAsync(pageNr, pageSize);
				return Ok(comments);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "En uventet feil oppsto ved forsøk på å hente kommentarer");
			}
		}


		// POST api/<CommentsCotroller>
		[HttpPost("{postId}", Name = "PostCommentOnAStatus")]
		public async Task<ActionResult<CommentDTO>> PostComment(int postId, CommentDTO commentDTO)
		{
			try
			{
				var context = HttpContext;
				if (!context.User.Identity.IsAuthenticated)
				{					
					return Unauthorized("Du må være innlogget for å legge til en kommentar");
				}

				int inloggedUserId = int.TryParse(HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0;

				var addedComment = await _commentService.AddCommentAsync(postId, commentDTO, inloggedUserId);

				if (addedComment == null)
				{
					return BadRequest("Kunne ikke legge til kommentar.");
				}
				return Ok(addedComment);

			}
			catch (Exception)
			{
				return StatusCode(500, "En feil oppsto ved forsøk på å legge til en ny kommentar");
			}
		}

		// PUT api/<CommentsCotroller>/5
		[HttpPut("{id}")]
		public async Task<ActionResult<CommentDTO>> UpdateComments(int commentId, [FromBody] CommentDTO commentDTO)
		{
			try
			{
				var updatedComment = await _commentService.UpdateCommentAsync(commentId, commentDTO, 0);
				if (updatedComment == null)
				{
					return NotFound();
				}
				return Ok(updatedComment);
			}
			catch (Exception)
			{
				return StatusCode(500, "En feil oppstod under oppdatering av kommentaren.");
			}
		}

		// DELETE api/<CommentsCotroller>/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<CommentDTO>>	DeleteComments(int commentId, int inloggedUser)
		{
			try
			{
				var deletedComment = await _commentService.DeleteCommentAsync(commentId, inloggedUser);
				if (deletedComment == null)
				{
					return NotFound();
				}
				return Ok(deletedComment);
			}
			catch (Exception)
			{
				return StatusCode(500, "En feil oppstod under slettingen av kommentaren");
			}
		}
	}
}
