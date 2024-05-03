using StudentBloggAPI.Mappers.Interfaces;
using StudentBloggAPI.Models.DTOs;
using StudentBloggAPI.Models.Entities;
using StudentBloggAPI.Repository.Interfaces;
using StudentBloggAPI.Services.Interfaces;

namespace StudentBloggAPI.Services;

public class UserService : IUserService
{
	private readonly IMapper<User, UserDTO> _userMapper;
	private readonly IMapper<User, UserRegistrationDTO> _userRegMapper;
	private readonly IUserRepository _userRepository;
	private readonly ILogger<UserService> _logger;


	public UserService(
		IMapper<User, UserDTO> userMapper,
		IMapper<User, UserRegistrationDTO> userRegMapper,
		IUserRepository userRepository,
		ILogger<UserService> logger)
	{
		_userMapper = userMapper;
		_userRepository = userRepository;
		_userRegMapper = userRegMapper;
		_logger = logger;
	}

	public IMapper<User, UserRegistrationDTO> UserRegMapper { get; }

	public async Task<UserDTO?> DeleteUserAsync(int deleteUserid, int loginUserId)
	{
		try
		{
			var inloggedUser = await _userRepository.GetUserByIdAsync(loginUserId);

			if (deleteUserid != loginUserId && !inloggedUser.IsAdminUser)
				throw new UnauthorizedAccessException($"Bruker {loginUserId} har ikke tilgang til å slette");

			var user = await _userRepository.GetUserByIdAsync(deleteUserid);
			if (user == null)
				return null;

			var res = await _userRepository.DeleteUserByIdAsync(deleteUserid);
			_logger.LogInformation("Bruker slettet {@user}", user);

			return res != null ? _userMapper.MapToDTO(user) : null;

		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Feil ved sletting av bruker {@bruker}", deleteUserid);
			throw;
		}
	}

	public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
	{
		try
		{
			var users = await _userRepository.GetUsersAsync();

			_logger.LogInformation("Hentet alle brukere ");

			var dtos = users.Select(users => _userMapper.MapToDTO(users)).ToList();
			return dtos;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex,"Feil ved henting av alle brukere");

			throw;
		}
	}

	public async Task<int> GetAuthenticatedIdAsync(string userName, string password)
	{
		var user = await _userRepository.GetUserByNameAsync(userName);
		if (user == null) return 0;


		// prøver å verifisere passordet mot lagret hash-verdi
		if (BCrypt.Net.BCrypt.Verify(password, user.HashedPassword))
		{
			return user.Id;
		}
		return 0;
	}

	public async Task<IEnumerable<UserDTO>> GetPageAsync(int pageNr, int pageSize)
	{
		try
		{
			var res = await _userRepository.GetPageAsync(pageNr, pageSize);

			_logger.LogInformation("Hentet brukere fra side {PageNr} med størrelse {PageSize}", pageNr, pageSize);

			return res.Select(usr => _userMapper.MapToDTO(usr)).ToList();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Feil ved henting av brukere fra side {PageNr} med størrelse {PageSize}", pageNr, pageSize);

			throw;
		}
	}

	public async Task<UserDTO?> GetUserByIdAsync(int id)
	{
		try
		{
			var res = await _userRepository.GetUserByIdAsync(id);

			_logger?.LogInformation("Forsøker å hente bruker med ID: {UserId}", id);

			return res != null ? _userMapper.MapToDTO(res) : null;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Feil ved henting av bruker {UserId}", id);
			throw;
		}
	}

	public async Task<UserDTO?> RegisterAsync(UserRegistrationDTO userRegDTO)
	{
		try
		{
			var user = _userRegMapper.MapToModel(userRegDTO);

			// Lage salt og hashverdier
			user.Salt = BCrypt.Net.BCrypt.GenerateSalt();
			user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegDTO.Password, user.Salt);

			// Legg til bruker i databasen
			var res = await _userRepository.AddUserAsync(user);

			// Logg registreringen
			_logger?.LogInformation("Ny bruker registrert: {@user}", res);

			// Returner DTO av den registrerte brukeren
			return _userMapper.MapToDTO(res!);
		}
		catch (Exception ex)
		{
			// Logg feil under registreringen
			_logger?.LogError(ex, "Feil under registrering av ny bruker");

			// Kast unntaket videre
			throw;
		}
	}


	public async Task<UserDTO?> UpdateUserAsync(int id, UserDTO userDto, int loginUserId)
	{
		try

	{
			var loginUser = await _userRepository.GetUserByIdAsync(loginUserId);
			if (loginUser == null)
			{
				_logger?.LogError("Bruker {LoginUserId} har ikke tilgang til å oppdatere, bruker ikke funnet", loginUserId);
				throw new UnauthorizedAccessException($"Bruker {loginUserId} har ikke tilgang til å oppdatere");
			}

			if (id != loginUserId && !loginUser.IsAdminUser)
			{
				_logger?.LogError("Bruker {LoginUserId} har ikke tilgang til å oppdatere andre brukere", loginUserId);
				throw new UnauthorizedAccessException($"Bruker {loginUserId} har ikke tilgang til å oppdatere");
			}

			var userToUpdate = await _userRepository.GetUserByIdAsync(id);
			if (userToUpdate == null)
			{
				_logger?.LogError("Bruker med ID {UserId} ble ikke funnet for oppdatering", id);
				return null;
			}

			var user = _userMapper.MapToModel(userDto);
			user.Id = id;

			// Oppdater brukeren i databasen
			await _userRepository.UpdateUserAsync(id, user);

			// Hent den oppdaterte brukeren fra databasen
			var updatedUser = await _userRepository.GetUserByIdAsync(id);

			if (updatedUser != null)
			{
				_logger?.LogInformation("Bruker med ID {UserId} ble oppdatert vellykket", id);

				// Returner de oppdaterte verdiene
				var updatedUserDTO = _userMapper.MapToDTO(updatedUser);
				return updatedUserDTO;
			}

			return null;
		}
    catch (Exception ex)
    {
			_logger?.LogError(ex, "Feil ved oppdatering av bruker {UserId}", id);
			throw;
		}
	}
}
		
