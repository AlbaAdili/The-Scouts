using The_Scouts.Models;

namespace The_Scouts.Repositories.Interfaces;

public interface IJobRepository
{
    Task<IEnumerable<Job>> GetAllAsync();
    Task<Job?> FindOneAsync(int id);
    Task AddAsync(Job job);
    Task UpdateAsync(Job job);
    Task DeleteAsync(int id);
    Task<IEnumerable<Job>> SearchAsync(string query);
}