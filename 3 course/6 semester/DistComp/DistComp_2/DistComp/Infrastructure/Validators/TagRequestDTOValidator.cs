using DistComp.DTO.RequestDTO;
using FluentValidation;

namespace DistComp.Infrastructure.Validators;

public class TagRequestDTOValidator : AbstractValidator<TagRequestDTO>
{
    public TagRequestDTOValidator()
    {
        RuleFor(dto => dto.Name).Length(2, 32);
    }
}