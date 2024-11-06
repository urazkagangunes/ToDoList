using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NUnit.Framework;
using ToDo.DataAccess.Abstracts;
using ToDo.Models.Dtos.Category.Request;
using ToDo.Models.Dtos.Category.Response;
using ToDo.Models.Entities;
using ToDo.Service.Concretes;
using Core.Responses;

namespace ToDo.Tests.ServiceTests
{
    [TestFixture]
    public class CategoryServiceTests
    {
        private Mock<ICategoryRepository> _mockCategoryRepository;
        private Mock<IMapper> _mockMapper;
        private CategoryService _categoryService;

        [SetUp]
        public void Setup()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockMapper = new Mock<IMapper>();
            _categoryService = new CategoryService(_mockCategoryRepository.Object, _mockMapper.Object);
        }

        [Test]
        public async Task AddAsync_ShouldAddCategory_WhenValidRequest()
{
            // Arrange
            var createRequest = new CreateCategoryRequest("Test Category");
            var categoryEntity = new Category { Id = 1, Name = createRequest.Name };
            var categoryResponse = new CategoryResponseDto { Id = categoryEntity.Id, Name = categoryEntity.Name };

            _mockMapper.Setup(m => m.Map<Category>(createRequest)).Returns(categoryEntity);
            _mockMapper.Setup(m => m.Map<CategoryResponseDto>(categoryEntity)).Returns(categoryResponse);

            // Act
            var result = await _categoryService.AddAsync(createRequest);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(categoryResponse.Name, result.Data.Name);
            _mockCategoryRepository.Verify(r => r.Add(It.IsAny<Category>()), Times.Once);
}

        //[Test]
        //public async Task GetAllAsync_ShouldReturnCategoryList()
        //{
        //    // Arrange
        //    var categories = new List<Category>
        //    {
        //        new Category { Id = 1, Name = "Category 1" },
        //        new Category { Id = 2, Name = "Category 2" }
        //    };

        //    var categoryResponseDtos = new List<CategoryResponseDto>
        //    {
        //        new CategoryResponseDto { Id = 1, Name = "Category 1" },
        //        new CategoryResponseDto { Id = 2, Name = "Category 2" }
        //    };

        //    _mockCategoryRepository.Setup(r => r.GetAll()).Returns(categories);
        //    _mockMapper.Setup(m => m.Map<List<CategoryResponseDto>>(categories)).Returns(categoryResponseDtos);

        //    // Act
        //    var result = await _categoryService.GetAllAsync();

        //    // Assert
        //    Assert.IsTrue(result.Success);
        //    Assert.AreEqual(2, result.Data.Count);
        //}

        [Test]
        public async Task GetByIdAsync_ShouldReturnCategory_WhenCategoryExists()
        {
            // Arrange
            var id = 1;
            var category = new Category { Id = id, Name = "Category 1" };
            var categoryResponseDto = new CategoryResponseDto { Id = id, Name = "Category 1" };

            _mockCategoryRepository.Setup(r => r.GetById(id)).Returns(category);
            _mockMapper.Setup(m => m.Map<CategoryResponseDto>(category)).Returns(categoryResponseDto);

            // Act
            var result = await _categoryService.GetByIdAsync(id);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(categoryResponseDto.Name, result.Data.Name);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnError_WhenCategoryNotFound()
        {
            // Arrange
            int id = 1;
            _mockCategoryRepository.Setup(r => r.GetById(id)).Returns((Category)null);

            // Act
            var result = await _categoryService.GetByIdAsync(id);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Category not found", result.Message);
        }

        [Test]
        public async Task RemoveAsync_ShouldRemoveCategory_WhenCategoryExists()
        {
            // Arrange
            var id = 1;
            var category = new Category { Id = id, Name = "Category 1" };
            var categoryResponseDto = new CategoryResponseDto { Id = id, Name = "Category 1" };

            _mockCategoryRepository.Setup(r => r.GetById(id)).Returns(category);
            _mockMapper.Setup(m => m.Map<CategoryResponseDto>(category)).Returns(categoryResponseDto);

            // Act
            var result = await _categoryService.RemoveAsync(id);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(categoryResponseDto.Name, result.Data.Name);
            _mockCategoryRepository.Verify(r => r.Remove(category), Times.Once);
        }

        [Test]
        public async Task RemoveAsync_ShouldReturnError_WhenCategoryNotFound()
        {
            // Arrange
            int id = 1;
            _mockCategoryRepository.Setup(r => r.GetById(id)).Returns((Category)null);

            // Act
            var result = await _categoryService.RemoveAsync(id);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Category not found", result.Message);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateCategory_WhenValidRequest()
        {
            // Arrange
            var updateRequest = new UpdateCategoryRequest(1, "Updated Category Name");
            var categoryEntity = new Category { Id = 1, Name = "Test Category" };
            var updatedCategory = new Category { Id = 1, Name = updateRequest.Name };
            var categoryResponse = new CategoryResponseDto { Id = updatedCategory.Id, Name = updatedCategory.Name };

            _mockMapper.Setup(m => m.Map<Category>(updateRequest)).Returns(updatedCategory);
            _mockMapper.Setup(m => m.Map<CategoryResponseDto>(updatedCategory)).Returns(categoryResponse);
            _mockCategoryRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns(categoryEntity);
            _mockCategoryRepository.Setup(r => r.Update(It.IsAny<Category>())).Verifiable();

            // Act
            var result = await _categoryService.UpdateAsync(updateRequest);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(categoryResponse.Name, result.Data.Name);
            _mockCategoryRepository.Verify(r => r.Update(It.IsAny<Category>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnError_WhenCategoryNotFound()
        {
            // Arrange
            var updateRequest = new UpdateCategoryRequest(1, "Updated Category"); // Use the constructor with both Id and Name
            _mockCategoryRepository.Setup(r => r.GetById(updateRequest.Id)).Returns((Category)null); // Mock the repository to return null for GetById

            // Act
            var result = await _categoryService.UpdateAsync(updateRequest);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Category not found", result.Message);
        }


    }
}
