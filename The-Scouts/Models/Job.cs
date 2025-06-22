using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace The_Scouts.Models;

public class Job
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(30)]
    public string Country { get; set; }

    [Required]
    [StringLength(30)]
    public string City { get; set; }

    [Required]
    [StringLength(30)]
    public string JobTitle { get; set; }

    [Required]
    public string JobDescription { get; set; }

    public ICollection<Application> Applications { get; set; }
    
}
