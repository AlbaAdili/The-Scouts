namespace The_ScoutsTests.ControllerTests;

using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;
using The_Scouts.Controllers;
using The_Scouts.DTOs;
using The_Scouts.Services.Interfaces;
using System;


    public class ContactControllerTests
    {
        private readonly Mock<IContactService> _mockService;
        private readonly IMemoryCache _cache;
        private readonly ContactController _controller;

        public ContactControllerTests()
        {
            _mockService = new Mock<IContactService>();
            _cache = new MemoryCache(new MemoryCacheOptions());
            _controller = new ContactController(_mockService.Object, _cache);
        }

        [Fact]
        public async Task GetAllMessages_ReturnsOk_WithCachedData()
        {
            var contacts = new List<ContactMessageDto>
            {
                new ContactMessageDto { Name = "Alice", Email = "alice@mail.com", Description = "Hi" }
            };
            _cache.Set("all_contacts", contacts);

            var result = await _controller.GetAllMessages();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<ContactMessageDto>>(okResult.Value);
            Assert.Single(value);
        }

        [Fact]
        public async Task GetAllMessages_FetchesFromService_WhenNotInCache()
        {
            var contacts = new List<ContactMessageDto>
            {
                new ContactMessageDto { Name = "Bob", Email = "bob@mail.com", Description = "Hello" }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(contacts);

            var result = await _controller.GetAllMessages();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<ContactMessageDto>>(okResult.Value);
            Assert.Single(value);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenFound()
        {
            var dto = new ContactMessageDto { Name = "Test", Email = "test@mail.com", Description = "Test desc" };
            _mockService.Setup(s => s.FindOneAsync(1)).ReturnsAsync(dto);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsType<ContactMessageDto>(okResult.Value);
            Assert.Equal("test@mail.com", value.Email);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenMissing()
        {
            _mockService.Setup(s => s.FindOneAsync(10)).ReturnsAsync((ContactMessageDto)null);

            var result = await _controller.GetById(10);

            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Message not found", notFound.Value);
        }

        [Fact]
        public async Task AddMessage_ReturnsOk_WhenValid()
        {
            var dto = new ContactMessageDto { Name = "New", Email = "new@mail.com", Description = "Help" };

            var result = await _controller.AddMessage(dto);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Message submitted successfully", ok.Value);
        }

        [Fact]
        public async Task AddMessage_ReturnsBadRequest_WhenInvalidModel()
        {
            _controller.ModelState.AddModelError("Name", "Required");
            var result = await _controller.AddMessage(new ContactMessageDto());

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }

