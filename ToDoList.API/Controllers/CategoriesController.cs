using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDo.Models.Dtos.Category.Request;
using ToDo.Service.Abstracts;

namespace ToDoList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var results = _categoryService.GetAllAsync();
            return Ok(results);
        }

        [HttpDelete("delete")]
        public IActionResult Remove([FromQuery] int id)
        {
            var result = _categoryService.RemoveAsync(id);
            return Ok(result);
        }

        [HttpPost("add")]
        public IActionResult Add(CreateCategoryRequest createCategoryRequest)
        {
            var result = _categoryService.AddAsync(createCategoryRequest);
            return Ok(result);
        }

        [HttpGet("getbyid/{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var result = _categoryService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] UpdateCategoryRequest updateCategoryRequest)
        {
            var result = _categoryService.UpdateAsync(updateCategoryRequest);
            return Ok(result);
        }
    }
}
