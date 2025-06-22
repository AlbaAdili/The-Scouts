namespace The_Scouts.DTOs;

public class ApplicationDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string Surname { get; set; }

    public string Email { get; set; }

    public string? PhoneNumber { get; set; }

    public IFormFile Resume { get; set; }

    public int JobId { get; set; }
}