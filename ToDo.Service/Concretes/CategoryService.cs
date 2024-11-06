using AutoMapper;
using Core.Responses;
using ToDo.DataAccess.Abstracts;
using ToDo.Models.Dtos.Category.Request;
using ToDo.Models.Dtos.Category.Response;
using ToDo.Models.Entities;
using ToDo.Service.Abstracts;

namespace ToDo.Service.Concretes;

public sealed class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<ReturnModel<CategoryResponseDto>> AddAsync(CreateCategoryRequest createCategoryRequest)
    {
        try
        {
            var categoryEntity = _mapper.Map<Category>(createCategoryRequest);
            _categoryRepository.Add(categoryEntity);

            var categoryResponse = _mapper.Map<CategoryResponseDto>(categoryEntity);
            return new ReturnModel<CategoryResponseDto>
            {
                Success = true,
                Data = categoryResponse
            };
        }
        catch (Exception ex)
        {
            return new ReturnModel<CategoryResponseDto>
            {
                Success = false,
                Message = $"Error adding category: {ex.Message}"
            };
        }
    }

    public async Task<ReturnModel<List<CategoryResponseDto>>> GetAllAsync()
    {
        try
        {
            var categories = _categoryRepository.GetAll();
            var categoryResponses = _mapper.Map<List<CategoryResponseDto>>(categories);

            return new ReturnModel<List<CategoryResponseDto>>
            {
                Success = true,
                Data = categoryResponses
            };
        }
        catch (Exception ex)
        {
            return new ReturnModel<List<CategoryResponseDto>>
            {
                Success = false,
                Message = $"Error fetching categories: {ex.Message}"
            };
        }
    }

    public async Task<ReturnModel<CategoryResponseDto>> GetByIdAsync(int id)
    {
        try
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
            {
                return new ReturnModel<CategoryResponseDto>
                {
                    Success = false,
                    Message = "Category not found"
                };
            }

            var categoryResponse = _mapper.Map<CategoryResponseDto>(category);
            return new ReturnModel<CategoryResponseDto>
            {
                Success = true,
                Data = categoryResponse
            };
        }
        catch (Exception ex)
        {
            return new ReturnModel<CategoryResponseDto>
            {
                Success = false,
                Message = $"Error fetching category by ID: {ex.Message}"
            };
        }
    }

    public async Task<ReturnModel<CategoryResponseDto>> RemoveAsync(int id)
    {
        try
        {
            var category =  _categoryRepository.GetById(id);
            if (category == null)
            {
                return new ReturnModel<CategoryResponseDto>
                {
                    Success = false,
                    Message = "Category not found"
                };
            }

            _categoryRepository.Remove(category);

            var categoryResponse = _mapper.Map<CategoryResponseDto>(category);
            return new ReturnModel<CategoryResponseDto>
            {
                Success = true,
                Data = categoryResponse
            };
        }
        catch (Exception ex)
        {
            return new ReturnModel<CategoryResponseDto>
            {
                Success = false,
                Message = $"Error removing category: {ex.Message}"
            };
        }
    }

    public async Task<ReturnModel<CategoryResponseDto>> UpdateAsync(UpdateCategoryRequest updateCategoryRequest)
    {
        try
        {
            var category = _categoryRepository.GetById(updateCategoryRequest.Id);
            if (category == null)
            {
                return new ReturnModel<CategoryResponseDto>
                {
                    Success = false,
                    Message = "Category not found"
                };
            }

            _mapper.Map(updateCategoryRequest, category);
            _categoryRepository.Update(category);

            var categoryResponse = _mapper.Map<CategoryResponseDto>(category);
            return new ReturnModel<CategoryResponseDto>
            {
                Success = true,
                Data = categoryResponse
            };
        }
        catch (Exception ex)
        {
            return new ReturnModel<CategoryResponseDto>
            {
                Success = false,
                Message = $"Error updating category: {ex.Message}"
            };
        }
    }
}
