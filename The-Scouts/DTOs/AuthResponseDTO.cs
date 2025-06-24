namespace The_Scouts.DTOs;

public class AuthResponseDTO
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string Role { get; set; } = null!;
}