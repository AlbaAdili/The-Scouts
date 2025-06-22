using The_Scouts.DTOs;
using The_Scouts.Models;

namespace The_Scouts.Services.Interfaces;

public interface IApplicationService
{
    Task<IEnumerable<ApplicationDto>> GetAllAsync();
    Task<ApplicationDto?> FindOneAsync(int id);
    Task<ApplicationDto?> FindByUserEmailAsync(string email);
    Task AddAsync(ApplicationDto dto, string userId, string resumePath);
    Task<Application?> UpdateStatusAsync(int applicationId, string status);
}

