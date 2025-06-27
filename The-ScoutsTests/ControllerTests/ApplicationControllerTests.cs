namespace The_ScoutsTests.ControllerTests;

using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using The_Scouts.Controllers;
using The_Scouts.DTOs;
using The_Scouts.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Linq;
using The_Scouts.Models;


public class ApplicationControllerTests
{
    private readonly Mock<IApplicationService> _mockService;
    private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
    private readonly IMemoryCache _cache;
    private readonly Mock<IWebHostEnvironment> _mockEnv;
    private readonly ApplicationController _controller;

    public ApplicationControllerTests()
    {
        _mockService = new Mock<IApplicationService>();

        var userStoreMock = new Mock<IUserStore<IdentityUser>>();
        _mockUserManager = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

        _cache = new MemoryCache(new MemoryCacheOptions());

        _mockEnv = new Mock<IWebHostEnvironment>();
        _mockEnv.Setup(e => e.WebRootPath).Returns(Directory.GetCurrentDirectory());

        _controller = new ApplicationController(_mockService.Object, _mockUserManager.Object, _cache, _mockEnv.Object);
    }

    [Fact]
    public async Task SubmitApplication_InvalidModel_ReturnsBadRequest()
    {
        _controller.ModelState.AddModelError("Resume", "Required");

        var result = await _controller.SubmitApplication(new ApplicationDto());

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task SubmitApplication_UserNotFound_ReturnsUnauthorized()
    {
        SetupHttpContextWithUser("user@example.com");
        _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((IdentityUser)null);

        var dto = new ApplicationDto { Resume = CreateMockFormFile("test.pdf") };
        var result = await _controller.SubmitApplication(dto);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task GetAllApplications_ReturnsCachedOrFreshList()
    {
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<ApplicationDto> { new ApplicationDto { Name = "Test" } });

        var result = await _controller.GetAllApplications();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var items = Assert.IsAssignableFrom<IEnumerable<ApplicationDto>>(okResult.Value);
        Assert.Single(items);
    }

    [Fact]
    public async Task GetById_NotFound_ReturnsNotFound()
    {
        _mockService.Setup(s => s.FindOneAsync(1)).ReturnsAsync((ApplicationDto)null);

        var result = await _controller.GetById(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_Exists_ReturnsOk()
    {
        var dto = new ApplicationDto { Id = 1 };
        _mockService.Setup(s => s.FindOneAsync(1)).ReturnsAsync(dto);

        var result = await _controller.GetById(1);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<ApplicationDto>(ok.Value);
    }
    

    [Fact]
    public async Task UpdateStatus_InvalidId_ReturnsNotFound()
    {
        _mockService.Setup(s => s.UpdateStatusAsync(99, "Accepted")).ReturnsAsync((Application)null);

        var result = await _controller.UpdateStatus(99, "Accepted");

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Application not found or invalid status.", notFound.Value);
    }

    // Helpers
    private void SetupHttpContextWithUser(string email)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, email)
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    private IFormFile CreateMockFormFile(string filename)
    {
        var content = "This is a dummy file.";
        var fileName = filename;
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(content);
        writer.Flush();
        ms.Position = 0;

        return new FormFile(ms, 0, ms.Length, "Resume", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/pdf"
        };
    }
}
