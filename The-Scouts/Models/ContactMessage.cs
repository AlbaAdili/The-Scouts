using System.ComponentModel.DataAnnotations;

namespace The_Scouts.Models;

public class ContactMessage
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

  
    [Required(ErrorMessage = "Email is required.")]
    [Display(Name = "Email")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required]
    public string Description { get; set; }

    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}