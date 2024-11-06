using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDo.Models.Dtos.ToDo.Request;
using ToDo.Service.Abstracts;

namespace ToDoList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDosController : ControllerBase
    {
        private readonly IToDoService _toDoService;

        public ToDosController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        [HttpGet("getall")]

        public IActionResult GetAll()
        {
            var result = _toDoService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] CreateToDoRequest dto)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var result = _toDoService.AddAsync(dto);
            return Ok(result);
        }

        [HttpGet("getbyid/{id}")]

        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _toDoService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] Guid id)
        {

            var result = _toDoService.RemoveAsync(id);
            return Ok(result);
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] UpdateToDoRequest dto)
        {
            var result = _toDoService.UpdateAsync(dto);
            return Ok(result);
        }
    }
}
