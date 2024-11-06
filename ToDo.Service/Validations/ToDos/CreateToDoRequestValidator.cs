using FluentValidation;
using ToDo.Models.Dtos.ToDo.Request;

namespace ToDo.Service.Validations.ToDos;

public class CreateToDoRequestValidator : AbstractValidator<CreateToDoRequest>
{
    public CreateToDoRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("ToDO Başlığı boş olamaz.")
            .MinimumLength(2).WithMessage("ToDo Başlığı minimum 2 Haneli olmalıdır.");

        RuleFor(x => x.Description).NotEmpty().WithMessage("İçerik boş olamaz.")
            .MinimumLength(10).WithMessage("İçerik minimum 10 haneli olmalıdır.");
    }
}
