using System;
using Microsoft.EntityFrameworkCore;
using The_Scouts.Models;
using TheScouts.Data;
using TheScouts.Repositories;


namespace The_Scouts.Repositories.Implementations
{
	public class NewsletterRepository : INewsletterRepository
	{
        private readonly AppDbContext _context;

        public NewsletterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Newsletter newsletter)
        {
            await _context.Newsletters.AddAsync(newsletter);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsEmailSubscribedAsync(string email)
        {
            return await _context.Newsletters.AnyAsync(n => n.Email == email);
        }
    }
}

