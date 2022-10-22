namespace r7.Models;

public class User
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string? Bio { get; set; }
    public string? PictureUrl { get; set; }
}