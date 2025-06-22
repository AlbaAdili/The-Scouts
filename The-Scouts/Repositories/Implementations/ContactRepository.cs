using Microsoft.EntityFrameworkCore;
using The_Scouts.Models;
using The_Scouts.Repositories.Interfaces;
using TheScouts.Data;

namespace The_Scouts.Repositories.Implementations;

public class ContactRepository : IContactRepository
{
    private readonly AppDbContext _context;

    public ContactRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ContactMessage>> GetAllAsync() => await _context.ContactMessages.ToListAsync();

    public async Task<ContactMessage?> FindOneAsync(int id) => await _context.ContactMessages.FindAsync(id);

    public async Task AddAsync(ContactMessage message)
    {
        await _context.ContactMessages.AddAsync(message);
        await _context.SaveChangesAsync();
    }
}
