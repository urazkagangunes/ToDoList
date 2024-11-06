using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NUnit.Framework;
using ToDo.DataAccess.Abstracts;
using ToDo.Models.Dtos.ToDo.Request;
using ToDo.Models.Dtos.ToDo.Response;
using ToDo.Models.Entities;
using ToDo.Service.Abstracts;
using ToDo.Service.Concretes;
using ToDo.Service.Rules;
using Core.Responses;

namespace ToDo.Tests.ServiceTests
{
    [TestFixture]
    public class ToDoServiceTests
    {
        private Mock<IToDoRepository> _mockToDoRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<ToDoBusinessRules> _mockBusinessRules;
        private IToDoService _toDoService;

        [SetUp]
        public void Setup()
        {
            _mockToDoRepository = new Mock<IToDoRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockBusinessRules = new Mock<ToDoBusinessRules>();
            _toDoService = new ToDoService(_mockToDoRepository.Object, _mockMapper.Object, _mockBusinessRules.Object);
        }


        [Test]
        public async Task GetByIdAsync_ShouldReturnToDo_WhenToDoExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var toDoEntity = new ToDo.Models.Entities.ToDo { Id = id, Title = "Test ToDo", Description = "Description" };
            var toDoResponseDto = new ToDoResponseDto { Id = id, Title = "Test ToDo", Description = "Description" };

            _mockToDoRepository.Setup(r => r.GetById(id)).Returns(toDoEntity);
            _mockMapper.Setup(m => m.Map<ToDoResponseDto>(toDoEntity)).Returns(toDoResponseDto);

            // Act
            var result = await _toDoService.GetByIdAsync(id);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(toDoResponseDto.Title, result.Data.Title);
        }

        [Test]
        public async Task RemoveAsync_ShouldRemoveToDo_WhenToDoExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var toDoEntity = new ToDo.Models.Entities.ToDo { Id = id, Title = "Test ToDo", Description = "Description" };
            var toDoResponseDto = new ToDoResponseDto { Id = id, Title = "Test ToDo", Description = "Description" };

            _mockToDoRepository.Setup(r => r.GetById(id)).Returns(toDoEntity);
            _mockMapper.Setup(m => m.Map<ToDoResponseDto>(toDoEntity)).Returns(toDoResponseDto);

            // Act
            var result = await _toDoService.RemoveAsync(id);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(toDoResponseDto.Title, result.Data.Title);
            _mockToDoRepository.Verify(r => r.Remove(toDoEntity), Times.Once);
        }

        //[Test]
        //public async Task UpdateAsync_ShouldUpdateToDo_WhenToDoExists()
        //{
        //    // Arrange
        //    var request = new UpdateToDoRequest { Id = Guid.NewGuid(), Title = "Updated Title", Description = "Updated Description" };
        //    var toDoEntity = new ToDo.Models.Entities.ToDo { Id = request.Id, Title = "Old Title", Description = "Old Description" };
        //    var updatedToDoResponseDto = new ToDoResponseDto { Id = request.Id, Title = request.Title, Description = request.Description };

        //    _mockToDoRepository.Setup(r => r.GetById(request.Id)).Returns(toDoEntity);
        //    _mockMapper.Setup(m => m.Map(request, toDoEntity));
        //    _mockMapper.Setup(m => m.Map<ToDoResponseDto>(toDoEntity)).Returns(updatedToDoResponseDto);

        //    // Act
        //    var result = await _toDoService.UpdateAsync(request);

        //    // Assert
        //    Assert.IsTrue(result.Success);
        //    Assert.AreEqual(updatedToDoResponseDto.Title, result.Data.Title);
        //    _mockToDoRepository.Verify(r => r.Update(toDoEntity), Times.Once);
        //}

        //[Test]
        //public async Task AddAsync_ShouldAddToDo_WhenValidRequest()
        //{
        //    // Arrange
        //    var request = new CreateToDoRequest
        //    {
        //        Title = "Test ToDo",
        //        Description = "Test Description",
        //        Priority = 1
        //    }; // Properties are directly assigned

        //    var toDoEntity = new ToDo.Models.Entities.ToDo { Id = Guid.NewGuid(), Title = request.Title, Description = request.Description };
        //    var responseDto = new ToDoResponseDto { Id = toDoEntity.Id, Title = toDoEntity.Title, Description = toDoEntity.Description };

        //    _mockMapper.Setup(m => m.Map<ToDo.Models.Entities.ToDo>(request)).Returns(toDoEntity);
        //    _mockMapper.Setup(m => m.Map<ToDoResponseDto>(toDoEntity)).Returns(responseDto);

        //    // Act
        //    var result = await _toDoService.AddAsync(request);

        //    // Assert
        //    Assert.IsTrue(result.Success);
        //    Assert.AreEqual(responseDto.Title, result.Data.Title);
        //    _mockToDoRepository.Verify(r => r.Add(It.IsAny<ToDo.Models.Entities.ToDo>()), Times.Once);
        //}


        //[Test]
        //public async Task GetAllAsync_ShouldReturnToDoList()
        //{
        //    // Arrange
        //    var toDoEntities = new List<ToDo.Models.Entities.ToDo>
        //    {
        //        new ToDo.Models.Entities.ToDo { Id = Guid.NewGuid(), Title = "Test ToDo 1", Description = "Description 1" },
        //        new ToDo.Models.Entities.ToDo { Id = Guid.NewGuid(), Title = "Test ToDo 2", Description = "Description 2" }
        //    };

        //    var toDoResponseDtos = new List<ToDoResponseDto>
        //    {
        //        new ToDoResponseDto { Id = toDoEntities[0].Id, Title = "Test ToDo 1", Description = "Description 1" },
        //        new ToDoResponseDto { Id = toDoEntities[1].Id, Title = "Test ToDo 2", Description = "Description 2" }
        //    };

        //    _mockToDoRepository.Setup(r => r.GetAll()).Returns(toDoEntities);
        //    _mockMapper.Setup(m => m.Map<List<ToDoResponseDto>>(toDoEntities)).Returns(toDoResponseDtos);

        //    // Act
        //    var result = await _toDoService.GetAllAsync();

        //    // Assert
        //    Assert.IsTrue(result.Success);
        //    Assert.AreEqual(2, result.Data.Count);
        //}

    }
}
