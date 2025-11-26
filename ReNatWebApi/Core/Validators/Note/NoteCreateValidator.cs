using Core.Models.Note;
using FluentValidation;

namespace Core.Validators.Note;

public class NoteCreateValidator : AbstractValidator<NoteCreateModel>
{
    public NoteCreateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Вкажіть назву завдання.")
            .MaximumLength(250).WithMessage("Максимальна довжина назви — 250 символів.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Максимальна довжина опису — 1000 символів.")
            .When(x => x.Description != null);

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Виберіть категорію.");

        RuleFor(x => x.Deadline)
            .GreaterThan(DateTime.UtcNow).WithMessage("Дедлайн не може бути у минулому.")
            .When(x => x.Deadline.HasValue);
    }
}
