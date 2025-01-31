using DistComp_1.Exceptions;
using DistComp_1.Models;
using DistComp_1.Repositories.Interfaces;

namespace DistComp_1.Repositories.Implementations;

public class InMemoryUserRepository : BaseInMemoryRepository<User>, IUserRepository
{
    /*
    // Индекс для поиска по логину
    private readonly Dictionary<string, long> _loginIndex = [];

    public override async Task<User> CreateAsync(User entity)
    {
        if (_loginIndex.ContainsKey(entity.Login))
        {
            throw new ConflictException(ErrorCodes.UserAlreadyExists, ErrorMessages.UserAlreadyExists(entity.Login));
        }

        var user = await base.CreateAsync(entity);
        _loginIndex.Add(user.Login, user.Id);

        return user;
    }

    public override async Task<User?> UpdateAsync(User entity)
    {
        if (_loginIndex.TryGetValue(entity.Login, out long value) && value != entity.Id)
        {
            throw new ConflictException(ErrorCodes.UserAlreadyExists, ErrorMessages.UserAlreadyExists(entity.Login));
        }

        var updatedUser = await base.UpdateAsync(entity);
        if (updatedUser != null)
        {
            if (_loginIndex.ContainsKey(entity.Login) && _loginIndex[entity.Login] == entity.Id)
            {
                return updatedUser;
            }

            _loginIndex.Remove(entity.Login);
            _loginIndex.Add(updatedUser.Login, updatedUser.Id);
        }

        return updatedUser;
    }

    public override async Task<User?> DeleteAsync(long id)
    {
        var user = await base.DeleteAsync(id);

        if (user != null)
        {
            _loginIndex.Remove(user.Login);
        }

        return user;
    }
    */
}