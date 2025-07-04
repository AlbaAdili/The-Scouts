using Microsoft.EntityFrameworkCore;
using The_Scouts.Models;
using The_Scouts.Repositories.Interfaces;
using TheScouts.Data;

namespace The_Scouts.Repositories.Implementations;

public class JobRepository : IJobRepository
{
    private readonly AppDbContext _context;
    public JobRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Job>> GetAllAsync() => await _context.Jobs.ToListAsync();

    public async Task<Job?> FindOneAsync(int id) => await _context.Jobs.FindAsync(id);

    public async Task AddAsync(Job job)
    {
        await _context.Jobs.AddAsync(job);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Job job)
    {
        _context.Jobs.Update(job);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var job = await FindOneAsync(id);
        if (job != null)
        {
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<IEnumerable<Job>> SearchAsync(string query)
    {
        return await _context.Jobs
            .Where(j => j.JobTitle.Contains(query) ||
                        j.City.Contains(query) ||
                        j.Country.Contains(query))
            .ToListAsync();
    }

}
