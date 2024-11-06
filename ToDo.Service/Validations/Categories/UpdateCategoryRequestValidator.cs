using FluentValidation;
using ToDo.Models.Dtos.Category.Request;

namespace ToDo.Service.Validations.Categories;

public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id alanı boş olamaz.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Kategori Adı Boş olamaz.");
    }
}
