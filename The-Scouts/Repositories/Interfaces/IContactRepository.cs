using The_Scouts.Models;

namespace The_Scouts.Repositories.Interfaces;

public interface IContactRepository
{
    Task<IEnumerable<ContactMessage>> GetAllAsync();
    Task<ContactMessage?> FindOneAsync(int id);
    Task AddAsync(ContactMessage message);
}