namespace The_ScoutsTests.ServiceTests;

using Xunit;
using Moq;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using The_Scouts.Services.Implementations;
using The_Scouts.Repositories.Interfaces;
using The_Scouts.Models;
using The_Scouts.DTOs;


    public class JobServiceTests
    {
        private readonly Mock<IJobRepository> _mockRepo;
        private readonly IMapper _mapper;
        private readonly JobService _service;

        public JobServiceTests()
        {
            _mockRepo = new Mock<IJobRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Job, JobDto>().ReverseMap();
            });
            _mapper = config.CreateMapper();

            _service = new JobService(_mockRepo.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedJobs()
        {
            var jobs = new List<Job>
            {
                new Job { JobTitle = "Backend Dev", City = "Berlin", Country = "Germany", JobDescription = "Develop APIs" }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(jobs);

            var result = await _service.GetAllAsync();

            Assert.Single(result);
            Assert.Equal("Backend Dev", result.First().JobTitle);
        }

        [Fact]
        public async Task FindOneAsync_ReturnsMappedJob_WhenFound()
        {
            var job = new Job { Id = 1, JobTitle = "Frontend Dev", City = "Paris", Country = "France", JobDescription = "UI Development" };
            _mockRepo.Setup(r => r.FindOneAsync(1)).ReturnsAsync(job);

            var result = await _service.FindOneAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Frontend Dev", result.JobTitle);
        }

        [Fact]
        public async Task FindOneAsync_ReturnsNull_WhenNotFound()
        {
            _mockRepo.Setup(r => r.FindOneAsync(99)).ReturnsAsync((Job)null);

            var result = await _service.FindOneAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_MapsAndCallsRepository()
        {
            var dto = new JobDto
            {
                JobTitle = "Data Analyst",
                City = "Zurich",
                Country = "Switzerland",
                JobDescription = "Analyze data"
            };

            await _service.AddAsync(dto);

            _mockRepo.Verify(r => r.AddAsync(It.Is<Job>(j =>
                j.JobTitle == "Data Analyst" &&
                j.City == "Zurich" &&
                j.Country == "Switzerland" &&
                j.JobDescription == "Analyze data"
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesIfJobExists()
        {
            var existingJob = new Job { Id = 1, JobTitle = "Old Title" };
            _mockRepo.Setup(r => r.FindOneAsync(1)).ReturnsAsync(existingJob);

            var dto = new JobDto { JobTitle = "New Title", City = "Skopje", Country = "Macedonia", JobDescription = "Updated Desc" };

            await _service.UpdateAsync(1, dto);

            _mockRepo.Verify(r => r.UpdateAsync(It.Is<Job>(j =>
                j.JobTitle == "New Title"
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_DoesNothingIfJobNotFound()
        {
            _mockRepo.Setup(r => r.FindOneAsync(404)).ReturnsAsync((Job)null);

            await _service.UpdateAsync(404, new JobDto { JobTitle = "None" });

            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Job>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepository()
        {
            await _service.DeleteAsync(5);

            _mockRepo.Verify(r => r.DeleteAsync(5), Times.Once);
        }
    }

