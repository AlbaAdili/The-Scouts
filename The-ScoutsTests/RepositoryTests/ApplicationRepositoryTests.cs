namespace The_ScoutsTests.RepositoryTests;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using The_Scouts.Models;
using The_Scouts.Repositories.Implementations;
using TheScouts.Data;
using Xunit;


    public class ApplicationRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly ApplicationRepository _repository;

        public ApplicationRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ApplicationRepoTests")
                .Options;

            _context = new AppDbContext(options);
            _repository = new ApplicationRepository(_context);

            SeedData();
        }

        private void SeedData()
        {
            _context.Jobs.RemoveRange(_context.Jobs);
            _context.Applications.RemoveRange(_context.Applications);
            _context.SaveChanges();

            var job = new Job
            {
                JobTitle = "Software Engineer",
                City = "Skopje",
                Country = "North Macedonia",
                JobDescription = "Responsible for backend development"
            };

            _context.Jobs.Add(job);
            _context.SaveChanges(); 

            _context.Applications.Add(new Application
            {
                Name = "Alba",
                Surname = "Adili",
                Email = "alba@example.com",
                PhoneNumber = "123456",
                ResumePath = "path/to/resume.pdf",
                JobId = job.Id,
                Status = ApplicationStatus.Submitted,
                SubmittedAt = DateTime.UtcNow
            });

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsApplications()
        {
            var result = await _repository.GetAllAsync();
            Assert.Single(result);
        }

        [Fact]
        public async Task FindOneAsync_ReturnsCorrectApplication()
        {
            var result = await _repository.FindOneAsync(1);
            Assert.NotNull(result);
            Assert.Equal("alba@example.com", result.Email);
        }

        [Fact]
        public async Task FindByUserIdAsync_ReturnsUserApplications()
        {
            var application = _context.Applications.First();
            application.UserId = "user123";
            await _context.SaveChangesAsync();

            var result = await _repository.FindByUserIdAsync("user123");
            Assert.Single(result);
        }

        [Fact]
        public async Task AddAsync_AddsApplication()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("AddAsyncTest")
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ApplicationRepository(context);
            
            context.Jobs.Add(new Job { Id = 1, JobTitle = "Dev", City = "City", Country = "Country", JobDescription = "Code" });
            await context.SaveChangesAsync();

            var app = new Application
            {
                Name = "Test",
                Surname = "User",
                Email = "test@mail.com",
                JobId = 1
            };

            await repo.AddAsync(app);
            Assert.Single(context.Applications);
        }


        [Fact]
        public async Task UpdateStatusAsync_UpdatesStatus_WhenValid()
        {
            var application = new Application
            {
                Name = "Test",
                Surname = "User",
                Email = "test@mail.com",
                JobId = 1,
                Status = ApplicationStatus.Submitted
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();
            var result = await _repository.UpdateStatusAsync(application.Id, "Finalized");
            Assert.NotNull(result);
            Assert.Equal(ApplicationStatus.Finalized, result.Status);
        }


        [Fact]
        public async Task UpdateStatusAsync_ReturnsNull_WhenInvalidStatus()
        {
            var application = new Application
            {
                Name = "Test",
                Surname = "User",
                Email = "test@mail.com",
                JobId = 1,
                Status = ApplicationStatus.Submitted
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();
            
            var result = await _repository.UpdateStatusAsync(application.Id, "INVALID_STATUS");
            Assert.Null(result);
        }

    }
