using AutoMapper;
using The_Scouts.DTOs;
using The_Scouts.Models;
using The_Scouts.Repositories.Interfaces;
using The_Scouts.Services.Interfaces;

namespace The_Scouts.Services.Implementations;

public class JobService : IJobService
{
    private readonly IJobRepository _repository;
    private readonly IMapper _mapper;
    public JobService(IJobRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<JobDto>> GetAllAsync()
    {
        var jobs = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<JobDto>>(jobs);
    }

    public async Task<JobDto?> FindOneAsync(int id)
    {
        var job = await _repository.FindOneAsync(id);
        return job == null ? null : _mapper.Map<JobDto>(job);
    }

    public async Task AddAsync(JobDto dto)
    {
        var job = _mapper.Map<Job>(dto);
        await _repository.AddAsync(job);
    }

    public async Task UpdateAsync(int id, JobDto dto)
    {
        var existing = await _repository.FindOneAsync(id);
        if (existing == null) return;
        _mapper.Map(dto, existing);
        await _repository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
    public async Task<IEnumerable<JobDto>> SearchAsync(string query)
    {
        var allJobs = await _repository.GetAllAsync(); 
        var filtered = allJobs.Where(j =>
            (!string.IsNullOrEmpty(j.JobTitle) && j.JobTitle.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
            (!string.IsNullOrEmpty(j.City) && j.City.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
            (!string.IsNullOrEmpty(j.Country) && j.Country.Contains(query, StringComparison.OrdinalIgnoreCase))
        );

        return filtered.Select(j => new JobDto
        {
            Id = j.Id,
            JobTitle = j.JobTitle,
            City = j.City,
            Country = j.Country,
            JobDescription = j.JobDescription
        });
    }

}