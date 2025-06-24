namespace The_Scouts.DTOs;

public class AuthRequestDTO
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool RememberMe { get; set; }
}