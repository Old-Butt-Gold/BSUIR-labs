using Discussion.DTO.Request;
using FluentValidation;

namespace Discussion.Infrastructure.Validators;

public class NoticeRequestDtoValidator : AbstractValidator<NoticeRequestDTO>
{
    public NoticeRequestDtoValidator()
    {
        RuleFor(dto => dto.Content).Length(2, 2048);
    }
}