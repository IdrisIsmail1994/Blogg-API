namespace StudentBloggAPI.Models.DTOs;

public record UserDTO(
	int Id,
	string? UserName,
	string? FirstName,
	string? LastName,
	string? Email,
	DateTime? Created);
