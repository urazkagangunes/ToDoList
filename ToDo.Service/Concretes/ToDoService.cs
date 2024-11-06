using AutoMapper;
using Core.Responses;
using ToDo.DataAccess.Abstracts;
using ToDo.Models.Dtos.ToDo.Request;
using ToDo.Models.Dtos.ToDo.Response;
using ToDo.Service.Abstracts;
using ToDo.Service.Rules;

namespace ToDo.Service.Concretes
{
    public sealed class ToDoService : IToDoService
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly IMapper _mapper;
        private readonly ToDoBusinessRules _toDoBusinessRules;

        public ToDoService(IToDoRepository toDoRepository, IMapper mapper, ToDoBusinessRules toDoBusinessRules)
        {
            _toDoRepository = toDoRepository;
            _mapper = mapper;
            _toDoBusinessRules = toDoBusinessRules;
        }

        public async Task<ReturnModel<ToDoResponseDto>> AddAsync(CreateToDoRequest createToDoRequest)
        {
            try
            {
                var toDoEntity = _mapper.Map<ToDo.Models.Entities.ToDo>(createToDoRequest);

                //// İş kurallarını doğrulama
                //await _toDoBusinessRules.CheckIfToDoTitleExists(toDoEntity.Title);

                _toDoRepository.Add(toDoEntity);

                var toDoResponse = _mapper.Map<ToDoResponseDto>(toDoEntity);
                return new ReturnModel<ToDoResponseDto>
                {
                    Success = true,
                    Data = toDoResponse
                };
            }
            catch (Exception ex)
            {
                return new ReturnModel<ToDoResponseDto>
                {
                    Success = false,
                    Message = $"Error adding ToDo: {ex.Message}"
                };
            }
        }

        public async Task<ReturnModel<List<ToDoResponseDto>>> GetAllAsync()
        {
            try
            {
                var toDoEntities = _toDoRepository.GetAll();
                var toDoResponses = _mapper.Map<List<ToDoResponseDto>>(toDoEntities);

                return new ReturnModel<List<ToDoResponseDto>>
                {
                    Success = true,
                    Data = toDoResponses
                };
            }
            catch (Exception ex)
            {
                return new ReturnModel<List<ToDoResponseDto>>
                {
                    Success = false,
                    Message = $"Error fetching ToDo list: {ex.Message}"
                };
            }
        }

        public async Task<ReturnModel<ToDoResponseDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var toDoEntity = _toDoRepository.GetById(id);
                if (toDoEntity == null)
                {
                    return new ReturnModel<ToDoResponseDto>
                    {
                        Success = false,
                        Message = "ToDo item not found"
                    };
                }

                var toDoResponse = _mapper.Map<ToDoResponseDto>(toDoEntity);
                return new ReturnModel<ToDoResponseDto>
                {
                    Success = true,
                    Data = toDoResponse
                };
            }
            catch (Exception ex)
            {
                return new ReturnModel<ToDoResponseDto>
                {
                    Success = false,
                    Message = $"Error fetching ToDo by ID: {ex.Message}"
                };
            }
        }

        public async Task<ReturnModel<ToDoResponseDto>> RemoveAsync(Guid id)
        {
            try
            {
                var toDoEntity = _toDoRepository.GetById(id);
                if (toDoEntity == null)
                {
                    return new ReturnModel<ToDoResponseDto>
                    {
                        Success = false,
                        Message = "ToDo item not found"
                    };
                }

                _toDoRepository.Remove(toDoEntity);

                var toDoResponse = _mapper.Map<ToDoResponseDto>(toDoEntity);
                return new ReturnModel<ToDoResponseDto>
                {
                    Success = true,
                    Data = toDoResponse
                };
            }
            catch (Exception ex)
            {
                return new ReturnModel<ToDoResponseDto>
                {
                    Success = false,
                    Message = $"Error removing ToDo: {ex.Message}"
                };
            }
        }

        public async Task<ReturnModel<ToDoResponseDto>> UpdateAsync(UpdateToDoRequest updateToDoRequest)
        {
            try
            {
                var toDoEntity = _toDoRepository.GetById(updateToDoRequest.Id);
                if (toDoEntity == null)
                {
                    return new ReturnModel<ToDoResponseDto>
                    {
                        Success = false,
                        Message = "ToDo item not found"
                    };
                }

                _mapper.Map(updateToDoRequest, toDoEntity);
                _toDoRepository.Update(toDoEntity);

                var toDoResponse = _mapper.Map<ToDoResponseDto>(toDoEntity);
                return new ReturnModel<ToDoResponseDto>
                {
                    Success = true,
                    Data = toDoResponse
                };
            }
            catch (Exception ex)
            {
                return new ReturnModel<ToDoResponseDto>
                {
                    Success = false,
                    Message = $"Error updating ToDo: {ex.Message}"
                };
            }
        }
    }
}
