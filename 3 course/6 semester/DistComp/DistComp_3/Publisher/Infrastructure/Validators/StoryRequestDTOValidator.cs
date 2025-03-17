using FluentValidation;
using Publisher.DTO.RequestDTO;

namespace Publisher.Infrastructure.Validators;

public class StoryRequestDTOValidator : AbstractValidator<StoryRequestDTO>
{
    public StoryRequestDTOValidator()
    {
        RuleFor(dto => dto.Title).Length(2, 64);
        RuleFor(dto => dto.Content).Length(4, 2048);
    }
}