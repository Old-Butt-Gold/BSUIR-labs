using Publisher.DTO.RequestDTO;
using Publisher.DTO.ResponseDTO;

namespace Publisher.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserResponseDTO>> GetUsersAsync();

    Task<UserResponseDTO> GetUserByIdAsync(long id);

    Task<UserResponseDTO> CreateUserAsync(UserRequestDTO user);

    Task<UserResponseDTO> UpdateUserAsync(UserRequestDTO user);

    Task DeleteUserAsync(long id);
}