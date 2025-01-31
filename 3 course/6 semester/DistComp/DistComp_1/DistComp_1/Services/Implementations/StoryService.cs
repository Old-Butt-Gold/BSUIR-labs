using AutoMapper;
using DistComp_1.DTO.RequestDTO;
using DistComp_1.DTO.ResponseDTO;
using DistComp_1.Exceptions;
using DistComp_1.Infrastructure.Validators;
using DistComp_1.Models;
using DistComp_1.Repositories.Interfaces;
using DistComp_1.Services.Interfaces;
using FluentValidation;

namespace DistComp_1.Services.Implementations;

public class StoryService : IStoryService
{
    private readonly IStoryRepository _storyRepository;
    private readonly IMapper _mapper;
    private readonly StoryRequestDTOValidator _validator;

    public StoryService(IStoryRepository storyRepository,
        IMapper mapper, StoryRequestDTOValidator validator)
    {
        _storyRepository = storyRepository;
        _mapper = mapper;
        _validator = validator;
    }
    
    public async Task<IEnumerable<StoryResponseDTO>> GetStoriesAsync()
    {
        var stories = await _storyRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<StoryResponseDTO>>(stories);
    }

    public async Task<StoryResponseDTO> GetStoryByIdAsync(long id)
    {
        var story = await _storyRepository.GetByIdAsync(id)
                    ?? throw new NotFoundException(ErrorCodes.StoryNotFound, ErrorMessages.StoryNotFoundMessage(id));
        return _mapper.Map<StoryResponseDTO>(story);
    }

    public async Task<StoryResponseDTO> CreateStoryAsync(StoryRequestDTO story)
    {
        await _validator.ValidateAndThrowAsync(story);
        var storyToCreate = _mapper.Map<Story>(story);
        
        storyToCreate.Created = DateTime.Now;
        storyToCreate.Modified = DateTime.Now;
        
        var createdStory = await _storyRepository.CreateAsync(storyToCreate);
        return _mapper.Map<StoryResponseDTO>(createdStory);
    }

    public async Task<StoryResponseDTO> UpdateStoryAsync(StoryRequestDTO story)
    {
        await _validator.ValidateAndThrowAsync(story);
        var storyToUpdate = _mapper.Map<Story>(story);
        
        storyToUpdate.Modified = DateTime.Now;
        
        var updatedStory = await _storyRepository.UpdateAsync(storyToUpdate)
                           ?? throw new NotFoundException(ErrorCodes.StoryNotFound, ErrorMessages.StoryNotFoundMessage(story.Id));
        return _mapper.Map<StoryResponseDTO>(updatedStory);
    }

    public async Task DeleteStoryAsync(long id)
    {
        if (await _storyRepository.DeleteAsync(id) is null)
        {
            throw new NotFoundException(ErrorCodes.StoryNotFound, ErrorMessages.StoryNotFoundMessage(id));
        }
    }
}