using DistComp_1.DTO.RequestDTO;
using FluentValidation;

namespace DistComp_1.Infrastructure.Validators;

public class NoticeRequestDTOValidator : AbstractValidator<NoticeRequestDTO>
{
    public NoticeRequestDTOValidator()
    {
        RuleFor(dto => dto.Content).Length(2, 2048);
    }
}