using System;
using The_Scouts.Models;

namespace The_Scouts.Services.Interfaces
{
	public interface INewsletterService
	{
        Task AddAsync(Newsletter newsletter);
        Task<bool> IsEmailSubscribedAsync(string email);
    }
}

