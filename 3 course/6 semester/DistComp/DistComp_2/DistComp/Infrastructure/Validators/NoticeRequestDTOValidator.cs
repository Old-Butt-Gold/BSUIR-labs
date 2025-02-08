using DistComp.DTO.RequestDTO;
using FluentValidation;

namespace DistComp.Infrastructure.Validators;

public class NoticeRequestDTOValidator : AbstractValidator<NoticeRequestDTO>
{
    public NoticeRequestDTOValidator()
    {
        RuleFor(dto => dto.Content).Length(2, 2048);
    }
}