using FluentValidation;
using Publisher.DTO.RequestDTO;

namespace Publisher.Infrastructure.Validators;

public class NoticeRequestDTOValidator : AbstractValidator<NoticeRequestDTO>
{
    public NoticeRequestDTOValidator()
    {
        RuleFor(dto => dto.Content).Length(2, 2048);
    }
}