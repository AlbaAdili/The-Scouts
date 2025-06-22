namespace The_ScoutsTests.RepositoryTests;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using The_Scouts.Models;
using The_Scouts.Repositories.Implementations;
using TheScouts.Data;
using Xunit;

    public class ContactRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_AddsMessage()
        {
            var context = GetDbContext(); 
            var repo = new ContactRepository(context);

            var message = new ContactMessage
            {
                Name = "Test",
                Email = "test@mail.com",
                Description = "Help"
            };

            await repo.AddAsync(message);

            var all = await context.ContactMessages.ToListAsync();
            Assert.Single(all);
        }


        [Fact]
        public async Task GetAllAsync_ReturnsAll()
        {
            var context = GetDbContext();
            context.ContactMessages.Add(new ContactMessage { Name = "A", Email = "a@mail.com", Description = "1" });
            context.ContactMessages.Add(new ContactMessage { Name = "B", Email = "b@mail.com", Description = "2" });
            await context.SaveChangesAsync();

            var repo = new ContactRepository(context);
            var result = await repo.GetAllAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task FindOneAsync_ReturnsCorrect()
        {
            var context = GetDbContext();
            context.ContactMessages.Add(new ContactMessage { Id = 5, Name = "Find", Email = "f@mail.com", Description = "Msg" });
            await context.SaveChangesAsync();

            var repo = new ContactRepository(context);
            var result = await repo.FindOneAsync(5);

            Assert.NotNull(result);
            Assert.Equal("Find", result.Name);
        }
    }

