using Microsoft.EntityFrameworkCore;
using The_Scouts.Models;
using The_Scouts.Repositories.Interfaces;
using TheScouts.Data;

namespace The_Scouts.Repositories.Implementations;

public class ApplicationRepository : IApplicationRepository
{
    private readonly AppDbContext _context;

    public ApplicationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Application>> GetAllAsync() =>
        await _context.Applications.Include(a => a.Job).ToListAsync();

    public async Task<Application?> FindOneAsync(int id) =>
        await _context.Applications.Include(a => a.Job).FirstOrDefaultAsync(a => a.Id == id);

    public async Task<IEnumerable<Application>> FindByUserIdAsync(string userId) =>
        await _context.Applications.Where(a => a.UserId == userId).ToListAsync();

    public async Task AddAsync(Application application)
    {
        _context.Applications.Add(application);
        await _context.SaveChangesAsync();
    }

    public async Task<Application?> UpdateStatusAsync(int applicationId, string status)
    {
        var app = await _context.Applications.FindAsync(applicationId);
        if (app == null) return null;

        if (!Enum.TryParse<ApplicationStatus>(status, ignoreCase: true, out var parsedStatus))
        {
            // Optionally log invalid status attempt here
            return null;
        }

        app.Status = parsedStatus;
        await _context.SaveChangesAsync();
        return app;
    }

}
