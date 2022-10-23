using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace r7.Models;

public class NewRepairItemRequest
{
    public string Name { get; set; }
    [Min(1)]
    public long UserId { get; set; }
    public string? Description { get; set; }
    public string? PictureUrl { get; set; }
    public string? Location { get; set; }
}