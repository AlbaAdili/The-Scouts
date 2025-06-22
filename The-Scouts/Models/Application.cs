using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace The_Scouts.Models;

public class Application
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Surname { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string? PhoneNumber { get; set; }

    // File path to resume PDF
    public string? ResumePath { get; set; }

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public int JobId { get; set; }
    public Job Job { get; set; }

    public string? UserId { get; set; }
    public IdentityUser? User { get; set; }
}
