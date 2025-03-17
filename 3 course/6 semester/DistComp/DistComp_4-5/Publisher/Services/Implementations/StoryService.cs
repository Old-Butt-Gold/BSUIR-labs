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

public class StoryService : IStoryService
{
    private readonly IStoryRepository _storyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly StoryRequestDTOValidator _validator;

    public StoryService(IStoryRepository storyRepository, IUserRepository userRepository,
        IMapper mapper, StoryRequestDTOValidator validator)
    {
        _storyRepository = storyRepository;
        _userRepository = userRepository;
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

        storyToCreate.UserId = story.UserId;
        storyToCreate.Created = DateTime.UtcNow;
        storyToCreate.Modified = DateTime.UtcNow;
        
        var createdStory = await _storyRepository.CreateAsync(storyToCreate);
        return _mapper.Map<StoryResponseDTO>(createdStory);
    }

    public async Task<StoryResponseDTO> UpdateStoryAsync(StoryRequestDTO story)
    {
        await _validator.ValidateAndThrowAsync(story);
        var storyToUpdate = _mapper.Map<Story>(story);
        
        storyToUpdate.Modified = DateTime.UtcNow;
        storyToUpdate.Created = DateTime.UtcNow;
        
        var updatedStory = await _storyRepository.UpdateAsync(storyToUpdate)
                           ?? throw new NotFoundException(ErrorCodes.StoryNotFound, ErrorMessages.StoryNotFoundMessage(story.Id));
        return _mapper.Map<StoryResponseDTO>(updatedStory);
    }

    public async Task DeleteStoryAsync(long id)
    {
        if (!await _storyRepository.DeleteAsync(id))
        {
            throw new NotFoundException(ErrorCodes.StoryNotFound, ErrorMessages.StoryNotFoundMessage(id));
        }
    }
}