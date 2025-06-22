using The_Scouts.DTOs;

namespace The_Scouts.Services.Interfaces;

public interface IJobService
{
    Task<IEnumerable<JobDto>> GetAllAsync();
    Task<JobDto?> FindOneAsync(int id);
    Task AddAsync(JobDto dto);
    Task UpdateAsync(int id, JobDto dto);
    Task DeleteAsync(int id);
}

