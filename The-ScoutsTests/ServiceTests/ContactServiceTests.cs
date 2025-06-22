namespace The_ScoutsTests.ServiceTests;

using Xunit;
using Moq;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using The_Scouts.Services.Implementations;
using The_Scouts.Repositories.Interfaces;
using The_Scouts.Models;
using The_Scouts.DTOs;

    public class ContactServiceTests
    {
        private readonly Mock<IContactRepository> _repoMock;
        private readonly IMapper _mapper;
        private readonly ContactService _service;

        public ContactServiceTests()
        {
            _repoMock = new Mock<IContactRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ContactMessage, ContactMessageDto>().ReverseMap();
            });
            _mapper = config.CreateMapper();

            _service = new ContactService(_repoMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedDtos()
        {
            var data = new List<ContactMessage>
            {
                new ContactMessage { Name = "Alba", Email = "alba@mail.com", Description = "Hello" }
            };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(data);

            var result = await _service.GetAllAsync();

            Assert.Single(result);
            Assert.Equal("alba@mail.com", result.First().Email);
        }

        [Fact]
        public async Task FindOneAsync_ReturnsMappedDto_WhenFound()
        {
            var contact = new ContactMessage { Id = 1, Name = "Test", Email = "test@mail.com" };
            _repoMock.Setup(r => r.FindOneAsync(1)).ReturnsAsync(contact);

            var result = await _service.FindOneAsync(1);

            Assert.NotNull(result);
            Assert.Equal("test@mail.com", result.Email);
        }

        [Fact]
        public async Task FindOneAsync_ReturnsNull_WhenNotFound()
        {
            _repoMock.Setup(r => r.FindOneAsync(999)).ReturnsAsync((ContactMessage)null);

            var result = await _service.FindOneAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_CallsRepositoryWithMappedEntity()
        {
            var dto = new ContactMessageDto
            {
                Name = "Jane",
                Email = "jane@mail.com",
                Description = "Help"
            };

            await _service.AddAsync(dto);

            _repoMock.Verify(r => r.AddAsync(It.Is<ContactMessage>(
                m => m.Email == "jane@mail.com" && m.Name == "Jane" && m.Description == "Help"
            )), Times.Once);
        }
    }

