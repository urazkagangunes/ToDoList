using FluentValidation;
using ToDo.Models.Dtos.Category.Request;

namespace ToDo.Service.Validations.Categories;

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Kategori Adı Boş olamaz.");
    }
}
