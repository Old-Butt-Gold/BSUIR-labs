using DistComp_1.DTO.RequestDTO;
using FluentValidation;

namespace DistComp_1.Infrastructure.Validators;

public class TagRequestDTOValidator : AbstractValidator<TagRequestDTO>
{
    public TagRequestDTOValidator()
    {
        RuleFor(dto => dto.Name).Length(2, 32);
    }
}