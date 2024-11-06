using FluentValidation;
using ToDo.Models.Dtos.ToDo.Request;

namespace ToDo.Service.Validations.ToDos;

public class UpdateToDoRequestValidator : AbstractValidator<UpdateToDoRequest>
{
    public UpdateToDoRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id Alanı boş olamaz.");


        RuleFor(x => x.Title).NotEmpty().WithMessage("Post Başlığı boş olamaz.")
            .MinimumLength(2).WithMessage("Post Başlığı minimum 2 Haneli olmalıdır.");

        RuleFor(x => x.Description).NotEmpty().WithMessage("İçerik boş olamaz.")
            .MinimumLength(10).WithMessage("İçerik minimum 10 haneli olmalıdır.");
    }
}
