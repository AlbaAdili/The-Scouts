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

    public async Task<IEnumerable<Application>> GetAllAsync() => await _context.Applications.Include(a => a.Job).ToListAsync();

    public async Task<Application?> FindOneAsync(int id) => await _context.Applications.Include(a => a.Job).FirstOrDefaultAsync(a => a.Id == id);

    public async Task<IEnumerable<Application>> FindByUserIdAsync(string userId)
    {
        return await _context.Applications.Where(a => a.UserId == userId).ToListAsync();
    }

    public async Task AddAsync(Application application)
    {
        await _context.Applications.AddAsync(application);
        await _context.SaveChangesAsync();
    }

    public async Task<Application?> UpdateStatusAsync(int applicationId, string status)
    {
        var app = await _context.Applications.FindAsync(applicationId);
        if (app == null) return null;

        if (Enum.TryParse<ApplicationStatus>(status, true, out var parsedStatus))
        {
            app.Status = parsedStatus;
            await _context.SaveChangesAsync();
            return app;
        }

        return null;
    }
    public async Task<IEnumerable<Application>> FindByUserEmailAsync(string email)
    {
        return await _context.Applications
            .Include(a => a.Job)
            .Where(a => a.Email == email)
            .ToListAsync();
    }

    public async Task<IEnumerable<Application>> FindByPositionIdAsync(int positionId)
    {
        return await _context.Applications
            .Include(a => a.Job)
            .Where(a => a.JobId == positionId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Application>> SearchAsync(string searchTerm)
    {
        return await _context.Applications
            .Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()) ||
                        a.Surname.ToLower().Contains(searchTerm.ToLower()))
            .Include(a => a.Job)
            .ToListAsync();
    }

}