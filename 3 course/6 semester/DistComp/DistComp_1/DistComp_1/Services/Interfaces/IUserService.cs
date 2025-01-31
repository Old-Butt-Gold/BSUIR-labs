using DistComp_1.DTO.RequestDTO;
using DistComp_1.DTO.ResponseDTO;

namespace DistComp_1.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserResponseDTO>> GetUsersAsync();

    Task<UserResponseDTO> GetUserByIdAsync(long id);

    Task<UserResponseDTO> CreateUserAsync(UserRequestDTO user);

    Task<UserResponseDTO> UpdateUserAsync(UserRequestDTO user);

    Task DeleteUserAsync(long id);
}