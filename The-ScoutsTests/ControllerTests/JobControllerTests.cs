namespace The_ScoutsTests.ControllerTests;

using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using The_Scouts.Controllers;
using The_Scouts.DTOs;
using The_Scouts.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

    public class JobControllerTests
    {
        private readonly Mock<IJobService> _jobServiceMock;
        private readonly IMemoryCache _memoryCache;
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly JobController _controller;

        public JobControllerTests()
        {
            _jobServiceMock = new Mock<IJobService>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null
            );
            _controller = new JobController(_jobServiceMock.Object, _memoryCache, _userManagerMock.Object);
        }

        [Fact]
        public async Task GetAllJobs_ReturnsOk()
        {
            var jobs = new List<JobDto> { new JobDto { JobTitle = "Test Job" } };
            _jobServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(jobs);

            var result = await _controller.GetAllJobs();

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsAssignableFrom<IEnumerable<JobDto>>(ok.Value);
        }

        [Fact]
        public async Task GetJobById_ReturnsJob_WhenExists()
        {
            var dto = new JobDto { JobTitle = "Developer" };
            _jobServiceMock.Setup(s => s.FindOneAsync(1)).ReturnsAsync(dto);

            var result = await _controller.GetJobById(1);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var job = Assert.IsType<JobDto>(ok.Value);
            Assert.Equal("Developer", job.JobTitle);
        }

        [Fact]
        public async Task GetJobById_ReturnsNotFound_WhenMissing()
        {
            _jobServiceMock.Setup(s => s.FindOneAsync(10)).ReturnsAsync((JobDto)null);

            var result = await _controller.GetJobById(10);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task AddJob_ReturnsOk_WhenValid()
        {
            var job = new JobDto { JobTitle = "New Job" };

            var result = await _controller.AddJob(job);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Job added successfully", ok.Value);
        }

        [Fact]
        public async Task AddJob_ReturnsBadRequest_WhenModelInvalid()
        {
            _controller.ModelState.AddModelError("Title", "Required");

            var result = await _controller.AddJob(new JobDto());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateJob_ReturnsOk_WhenValid()
        {
            var job = new JobDto { JobTitle = "Updated" };
            _jobServiceMock.Setup(s => s.FindOneAsync(1)).ReturnsAsync(job);

            var result = await _controller.UpdateJob(1, job);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Job updated successfully", ok.Value);
        }

        [Fact]
        public async Task UpdateJob_ReturnsNotFound_WhenMissing()
        {
            _jobServiceMock.Setup(s => s.FindOneAsync(99)).ReturnsAsync((JobDto)null);

            var result = await _controller.UpdateJob(99, new JobDto());

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Job not found", notFound.Value);
        }

        [Fact]
        public async Task DeleteJob_ReturnsOk_WhenFound()
        {
            var job = new JobDto { JobTitle = "ToDelete" };
            _jobServiceMock.Setup(s => s.FindOneAsync(1)).ReturnsAsync(job);

            var result = await _controller.DeleteJob(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Job deleted successfully", ok.Value);
        }

        [Fact]
        public async Task DeleteJob_ReturnsNotFound_WhenMissing()
        {
            _jobServiceMock.Setup(s => s.FindOneAsync(5)).ReturnsAsync((JobDto)null);

            var result = await _controller.DeleteJob(5);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Job not found", notFound.Value);
        }
    }

