using The_Scouts.Models;

namespace The_Scouts.Repositories.Interfaces;

public interface IApplicationRepository
{
    Task<IEnumerable<Application>> GetAllAsync();
    Task<Application?> FindOneAsync(int id);
    Task<IEnumerable<Application>> FindByUserIdAsync(string userId);
    Task AddAsync(Application application);
    Task<Application?> UpdateStatusAsync(int applicationId, string status);
}