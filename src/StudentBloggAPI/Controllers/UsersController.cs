using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudentBloggAPI.Models.DTOs;
using StudentBloggAPI.Models.Entities;
using StudentBloggAPI.Services.Interfaces;

namespace StudentBloggAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
       _userService = userService;
    }
    //https://localhost:7064/api/v1/User
    [HttpGet (Name = "GetAllUsers")]
	public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersAsync(int pageNr = 1, int pageSize = 10)
	{
        
		return Ok(await _userService.GetPageAsync(pageNr, pageSize));
	}

    [HttpGet("{id}", Name = "GetUsersById")]
    public async Task<ActionResult<UserDTO>> GetUserByIdAsync(int id)
    {
        var res = await _userService.GetUserByIdAsync(id);
        return res != null ? Ok(res) : NotFound("Fant ikke bruker");
        
    }

    [HttpDelete("{id}", Name = "DeleteUser")]
    public async Task<ActionResult<UserDTO>> DeleteUserAsync(int id)
    {
        int loginUserId = (int)this.HttpContext.Items["UserId"]!;
		var res = await _userService.DeleteUserAsync(id, loginUserId);
		return res != null ? Ok(res) : BadRequest("Kan ikke slette bruker");
	}

    [HttpPut("{id}", Name = "UpdateUser")] 
    public async Task<ActionResult<UserDTO>> UpdateUserAsync(int id, UserDTO dto)
    {

        var loginUserId = (int)(HttpContext.Items["UserId"]!);

		var res = await _userService.UpdateUserAsync(id, dto, loginUserId);
		return res != null ? Ok(res) : NotFound("Fant ikke bruker");
	}

    [HttpPost("register", Name = "AddUser")]
    public async Task<ActionResult<UserDTO>> AddUserAsync(UserRegistrationDTO userRegDTO)
    {
        //modellbinding har skjedd
        //validering har skjedd

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);  
        }
        var userDTO = await _userService.RegisterAsync(userRegDTO);

        return userDTO != null
            ? Ok(userDTO)
            : BadRequest("Klarte ikke registrere ny bruker");
            
    }

}
