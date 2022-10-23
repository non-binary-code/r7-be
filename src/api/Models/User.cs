namespace r7.Models;

public class User
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string? Bio { get; set; }
    public string? PictureUrl { get; set; }
    public string? Location { get; set; }
    public bool WillRecover { get; set; }
    public bool AllowBookings { get; set; }
    public string? Availability { get; set; }
    public decimal? DistanceWillTravel { get; set; }
}