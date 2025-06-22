namespace The_ScoutsTests.RepositoryTests;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using The_Scouts.Models;
using The_Scouts.Repositories.Implementations;
using TheScouts.Data;
using Xunit;


    public class JobRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task AddAsync_AddsJobToDatabase()
        {
            var context = GetDbContext();
            var repo = new JobRepository(context);

            var job = new Job { JobTitle = "Tester", City = "Tetovo", Country = "Macedonia", JobDescription = "QA work" };
            await repo.AddAsync(job);

            var jobs = await context.Jobs.ToListAsync();
            Assert.Single(jobs);
            Assert.Equal("Tester", jobs.First().JobTitle);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllJobs()
        {
            var context = GetDbContext();
            context.Jobs.Add(new Job { JobTitle = "Dev", City = "Skopje", Country = "Macedonia", JobDescription = "Dev work" });
            context.Jobs.Add(new Job { JobTitle = "Tester", City = "Tetovo", Country = "Macedonia", JobDescription = "QA work" });
            await context.SaveChangesAsync();

            var repo = new JobRepository(context);
            var jobs = await repo.GetAllAsync();

            Assert.Equal(2, jobs.Count());
        }

        [Fact]
        public async Task FindOneAsync_ReturnsCorrectJob()
        {
            var context = GetDbContext();
            var job = new Job { JobTitle = "Dev", City = "Skopje", Country = "Macedonia", JobDescription = "Backend" };
            context.Jobs.Add(job);
            await context.SaveChangesAsync();

            var repo = new JobRepository(context);
            var result = await repo.FindOneAsync(job.Id);

            Assert.NotNull(result);
            Assert.Equal("Dev", result.JobTitle);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesJob()
        {
            var context = GetDbContext();
            var job = new Job { JobTitle = "Old", City = "Old", Country = "Old", JobDescription = "Old" };
            context.Jobs.Add(job);
            await context.SaveChangesAsync();

            job.JobTitle = "New";
            job.JobDescription = "Updated";
            var repo = new JobRepository(context);
            await repo.UpdateAsync(job);

            var updated = await context.Jobs.FindAsync(job.Id);
            Assert.Equal("New", updated.JobTitle);
            Assert.Equal("Updated", updated.JobDescription);
        }

        [Fact]
        public async Task DeleteAsync_RemovesJob()
        {
            var context = GetDbContext();
            var job = new Job { JobTitle = "ToDelete", City = "City", Country = "Country", JobDescription = "Delete this" };
            context.Jobs.Add(job);
            await context.SaveChangesAsync();

            var repo = new JobRepository(context);
            await repo.DeleteAsync(job.Id);

            var result = await context.Jobs.FindAsync(job.Id);
            Assert.Null(result);
        }
    }

