using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace r7.Models;

public class NewRecycleItemRequest
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? PictureUrl { get; set; }
    public decimal? Weight { get; set; }
    public string? Dimensions { get; set; }
    public bool Compostable { get; set; }
    public string? Location { get; set; }
    public string? RecycleLocation { get; set; }
    public decimal? Distance { get; set; }
    [Min(1)]
    public long UserId { get; set; }
}