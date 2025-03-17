using AutoMapper;
using FluentValidation;
using Publisher.DTO.RequestDTO;
using Publisher.DTO.ResponseDTO;
using Publisher.Exceptions;
using Publisher.Infrastructure.Validators;
using Publisher.Models;
using Publisher.Repositories.Interfaces;
using Publisher.Services.Interfaces;

namespace Publisher.Services.Implementations;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly IMapper _mapper;
    private readonly TagRequestDTOValidator _validator;
    
    public TagService(ITagRepository tagRepository, 
        IMapper mapper, TagRequestDTOValidator validator)
    {
        _tagRepository = tagRepository;
        _mapper = mapper;
        _validator = validator;
    }
    
    public async Task<IEnumerable<TagResponseDTO>> GetTagsAsync()
    {
        var tags = await _tagRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<TagResponseDTO>>(tags);
    }

    public async Task<TagResponseDTO> GetTagByIdAsync(long id)
    {
        var tag = await _tagRepository.GetByIdAsync(id)
                      ?? throw new NotFoundException(ErrorCodes.TagNotFound, ErrorMessages.TagNotFoundMessage(id));
        return _mapper.Map<TagResponseDTO>(tag);
    }

    public async Task<TagResponseDTO> CreateTagAsync(TagRequestDTO tag)
    {
        await _validator.ValidateAndThrowAsync(tag);
        var tagToCreate = _mapper.Map<Tag>(tag);
        var createdTag = await _tagRepository.CreateAsync(tagToCreate);
        return _mapper.Map<TagResponseDTO>(createdTag);
    }

    public async Task<TagResponseDTO> UpdateTagAsync(TagRequestDTO tag)
    {
        await _validator.ValidateAndThrowAsync(tag);
        var tagToUpdate = _mapper.Map<Tag>(tag);
        var updatedTag = await _tagRepository.UpdateAsync(tagToUpdate)
                             ?? throw new NotFoundException(ErrorCodes.TagNotFound, ErrorMessages.TagNotFoundMessage(tag.Id));
        return _mapper.Map<TagResponseDTO>(updatedTag);
    }

    public async Task DeleteTagAsync(long id)
    {
        if (!await _tagRepository.DeleteAsync(id))
        {
            throw new NotFoundException(ErrorCodes.TagNotFound, ErrorMessages.TagNotFoundMessage(id));
        }
    }
}