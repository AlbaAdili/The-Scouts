using AutoMapper;
using The_Scouts.DTOs;
using The_Scouts.Models;
using The_Scouts.Repositories.Interfaces;
using The_Scouts.Services.Interfaces;

namespace The_Scouts.Services.Implementations;

public class ApplicationService : IApplicationService
{
    private readonly IApplicationRepository _repository;
    private readonly IMapper _mapper;

    public ApplicationService(IApplicationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ApplicationDto>> GetAllAsync()
    {
        var apps = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ApplicationDto>>(apps);
    }

    public async Task<ApplicationDto?> FindOneAsync(int id)
    {
        var app = await _repository.FindOneAsync(id);
        return app == null ? null : _mapper.Map<ApplicationDto>(app);
    }

    public async Task<ApplicationDto?> FindByUserEmailAsync(string email)
    {
        var apps = await _repository.GetAllAsync();
        var app = apps.FirstOrDefault(a => a.Email == email);
        return app == null ? null : _mapper.Map<ApplicationDto>(app);
    }

    public async Task AddAsync(ApplicationDto dto, string userId, string resumePath)
    {
        var application = _mapper.Map<Application>(dto);
        application.UserId = userId;
        application.ResumePath = resumePath;
        application.Status = ApplicationStatus.Submitted;
        await _repository.AddAsync(application);
    }

    public async Task<Application?> UpdateStatusAsync(int applicationId, string status)
    {
        return await _repository.UpdateStatusAsync(applicationId, status);
    }
    public async Task<IEnumerable<ApplicationDto>> SearchAsync(string searchTerm)
    {
        var apps = await _repository.GetAllAsync();
        if (string.IsNullOrWhiteSpace(searchTerm))
            return _mapper.Map<IEnumerable<ApplicationDto>>(apps);

        var filtered = apps.Where(a =>
            a.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            a.Surname.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
        );

        return _mapper.Map<IEnumerable<ApplicationDto>>(filtered);
    }
    public async Task<IEnumerable<ApplicationDto>> FindApplicationsByUserEmailAsync(string email)
    {
        var apps = await _repository.FindByUserEmailAsync(email);
        return _mapper.Map<IEnumerable<ApplicationDto>>(apps);
    }

    public async Task<IEnumerable<ApplicationDto>> FindApplicationsAsync(int positionId)
    {
        var apps = await _repository.FindByPositionIdAsync(positionId);
        return _mapper.Map<IEnumerable<ApplicationDto>>(apps);
    }


}