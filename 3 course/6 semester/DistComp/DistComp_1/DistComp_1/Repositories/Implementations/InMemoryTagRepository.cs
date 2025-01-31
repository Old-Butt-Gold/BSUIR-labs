using DistComp_1.Exceptions;
using DistComp_1.Models;
using DistComp_1.Repositories.Interfaces;

namespace DistComp_1.Repositories.Implementations;

public class InMemoryTagRepository : BaseInMemoryRepository<Tag>, ITagRepository
{
    /*
    // Индекс для поиска по имени тега
    private readonly Dictionary<string, long> _nameIndex = [];

    public override async Task<Tag> CreateAsync(Tag entity)
    {
        if (_nameIndex.ContainsKey(entity.Name))
        {
            throw new ConflictException(ErrorCodes.TagAlreadyExists, 
                ErrorMessages.TagAlreadyExists(entity.Name));
        }

        var tag = await base.CreateAsync(entity);
        _nameIndex.Add(tag.Name, tag.Id);

        return tag;
    }

    public override async Task<Tag?> UpdateAsync(Tag entity)
    {
        if (_nameIndex.TryGetValue(entity.Name, out long value) && value != entity.Id)
        {
            throw new ConflictException(ErrorCodes.TagAlreadyExists, 
                ErrorMessages.TagAlreadyExists(entity.Name));
        }

        var updatedTag = await base.UpdateAsync(entity);
        if (updatedTag != null)
        {
            if (_nameIndex.ContainsKey(entity.Name) && _nameIndex[entity.Name] == entity.Id)
            {
                return updatedTag;
            }

            _nameIndex.Remove(entity.Name);
            _nameIndex.Add(updatedTag.Name, updatedTag.Id);
        }

        return updatedTag;
    }

    public override async Task<Tag?> DeleteAsync(long id)
    {
        var tag = await base.DeleteAsync(id);

        if (tag != null)
        {
            _nameIndex.Remove(tag.Name);
        }

        return tag;
    }
    */
}