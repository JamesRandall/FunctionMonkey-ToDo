using FluentValidation;
using ToDo.Commands;

namespace ToDo.Validators
{
    public class AddToDoItemCommandValidator : AbstractValidator<AddToDoItemCommand>
    {
        public AddToDoItemCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Title).NotEmpty().MaximumLength(128);
        }
    }
}