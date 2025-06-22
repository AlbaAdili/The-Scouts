using AutoMapper;
using The_Scouts.DTOs;
using The_Scouts.Models;
using The_Scouts.Repositories.Interfaces;
using The_Scouts.Services.Interfaces;

namespace The_Scouts.Services.Implementations;

public class ContactService : IContactService
{
    private readonly IContactRepository _repository;
    private readonly IMapper _mapper;

    public ContactService(IContactRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ContactMessageDto>> GetAllAsync()
    {
        var contacts = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ContactMessageDto>>(contacts);
    }

    public async Task<ContactMessageDto?> FindOneAsync(int id)
    {
        var contact = await _repository.FindOneAsync(id);
        return contact == null ? null : _mapper.Map<ContactMessageDto>(contact);
    }

    public async Task AddAsync(ContactMessageDto dto)
    {
        var message = _mapper.Map<ContactMessage>(dto);
        await _repository.AddAsync(message);
    }
}


