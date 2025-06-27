using System;
using The_Scouts.Models;
using The_Scouts.Services.Interfaces;
using TheScouts.Repositories;

namespace The_Scouts.Services.Implementations
{
	public class NewsletterService : INewsletterService
	{
        private readonly INewsletterRepository _repository;

        public NewsletterService(INewsletterRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(Newsletter newsletter)
        {
            await _repository.AddAsync(newsletter);
        }

        public async Task<bool> IsEmailSubscribedAsync(string email)
        {
            return await _repository.IsEmailSubscribedAsync(email);
        }
    }
}

