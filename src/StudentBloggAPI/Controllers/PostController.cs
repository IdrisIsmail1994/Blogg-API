using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentBloggAPI.Models.DTOs;
using StudentBloggAPI.Repository.Interfaces;
using StudentBloggAPI.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentBloggAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
	private readonly IPostService _postService;
    public PostController(IPostService userService)
    {
		_postService = userService;
    }
    // GET: api/<PostController>
    [HttpGet (Name = "GetAllPosts")]
	public async Task<ActionResult<IEnumerable<PostDTO>>> GetAllPostsAsync()
	{
		return Ok(await _postService.GetAllPostsAsync());
	}

	// GET api/<PostController>/5
	[HttpGet("{postId}", Name = "GetPostById")]
	public async Task<ActionResult<PostDTO>> GetPostById(int postId)
	{
		var post = await _postService.GetPostByIdAsync(postId);
		return post != null ? Ok(post) : NotFound("Fant ikke bruker");
	}

	// POST api/<PostController>
	[HttpPost]
	public async Task<ActionResult<PostDTO>> PostCommentAsync([FromBody] PostDTO postDTO)
	{
		if (postDTO == null)
		{
			return BadRequest("Ugyldig innlegg");
		}

		var addedPost = await _postService.AddPostAsync(postDTO);

		if(addedPost != null) 
		{
			return CreatedAtAction(nameof(GetPostById), new {addedPost.PostId}, addedPost);
		}

		return BadRequest("Feil ved opprettning av innlegg");
	}

	// PUT api/<PostController>/5
	[HttpPut("{id}")]
	public void Put(int id, [FromBody] string value)
	{
	}

	// DELETE api/<PostController>/5
	[HttpDelete("{id}")]
	public void Delete(int id)
	{

	}
}
