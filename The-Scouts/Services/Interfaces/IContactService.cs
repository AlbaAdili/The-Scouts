using The_Scouts.DTOs;
using The_Scouts.Models;

namespace The_Scouts.Services.Interfaces;

public interface IContactService
{
    Task<IEnumerable<ContactMessageDto>> GetAllAsync();
    Task<ContactMessageDto?> FindOneAsync(int id);
    Task AddAsync(ContactMessageDto dto);
}