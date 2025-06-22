namespace The_ScoutsTests.ServiceTests;

using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using The_Scouts.Services.Implementations;
using The_Scouts.DTOs;
using The_Scouts.Models;
using The_Scouts.Repositories.Interfaces;

    public class ApplicationServiceTests
    {
        private readonly Mock<IApplicationRepository> _repoMock;
        private readonly IMapper _mapper;
        private readonly ApplicationService _service;

        public ApplicationServiceTests()
        {
            _repoMock = new Mock<IApplicationRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Application, ApplicationDto>().ReverseMap();
            });
            _mapper = config.CreateMapper();

            _service = new ApplicationService(_repoMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedDtos()
        {
            var apps = new List<Application>
            {
                new Application { JobId = 1, Email = "test@mail.com" }
            };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(apps);

            var result = await _service.GetAllAsync();

            Assert.Single(result);
            Assert.Equal("test@mail.com", result.First().Email);
        }

        [Fact]
        public async Task FindOneAsync_ReturnsDto_WhenFound()
        {
            var app = new Application { JobId = 2, Email = "a@a.com" };
            _repoMock.Setup(r => r.FindOneAsync(2)).ReturnsAsync(app);

            var result = await _service.FindOneAsync(2);

            Assert.NotNull(result);
            Assert.Equal("a@a.com", result.Email);
        }

        [Fact]
        public async Task FindOneAsync_ReturnsNull_WhenNotFound()
        {
            _repoMock.Setup(r => r.FindOneAsync(100)).ReturnsAsync((Application)null);

            var result = await _service.FindOneAsync(100);

            Assert.Null(result);
        }

        [Fact]
        public async Task FindByUserEmailAsync_ReturnsDto_WhenMatchExists()
        {
            var apps = new List<Application>
            {
                new Application { Email = "user@mail.com" },
                new Application { Email = "other@mail.com" }
            };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(apps);

            var result = await _service.FindByUserEmailAsync("user@mail.com");

            Assert.NotNull(result);
            Assert.Equal("user@mail.com", result.Email);
        }

        [Fact]
        public async Task FindByUserEmailAsync_ReturnsNull_WhenNoMatch()
        {
            var apps = new List<Application>();
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(apps);

            var result = await _service.FindByUserEmailAsync("missing@mail.com");

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_SetsUserIdAndResumePathAndCallsRepo()
        {
            var dto = new ApplicationDto { Email = "user@mail.com", JobId = 1 };
            var userId = "user123";
            var resumePath = "/files/resume.pdf";

            await _service.AddAsync(dto, userId, resumePath);

            _repoMock.Verify(r => r.AddAsync(It.Is<Application>(
                a => a.UserId == userId &&
                     a.ResumePath == resumePath &&
                     a.Status == ApplicationStatus.Submitted
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_CallsRepository()
        {
            var application = new Application { Id = 1, Status = ApplicationStatus.Finalized };
            _repoMock.Setup(r => r.UpdateStatusAsync(1, "Finalized")).ReturnsAsync(application);

            var result = await _service.UpdateStatusAsync(1, "Finalized");

            Assert.NotNull(result);
            Assert.Equal(ApplicationStatus.Finalized, result.Status);
        }
    }

