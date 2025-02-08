using DistComp.DTO.RequestDTO;
using DistComp.DTO.ResponseDTO;

namespace DistComp.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserResponseDTO>> GetUsersAsync();

    Task<UserResponseDTO> GetUserByIdAsync(long id);

    Task<UserResponseDTO> CreateUserAsync(UserRequestDTO user);

    Task<UserResponseDTO> UpdateUserAsync(UserRequestDTO user);

    Task DeleteUserAsync(long id);
}