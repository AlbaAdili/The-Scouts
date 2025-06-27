using System;
using The_Scouts.Models;

namespace TheScouts.Repositories
{
	public interface INewsletterRepository
	{
        Task AddAsync(Newsletter newsletter);
        Task<bool> IsEmailSubscribedAsync(string email);
    }
}

