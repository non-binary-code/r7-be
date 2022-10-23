using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace r7.Models;

public class NewReuseItemRequest
{
    public string Name { get; set; }
    [Min(1)]
    public long UserId { get; set; }
    public string? Description { get; set; }
    public string? PictureUrl { get; set; }
    public string? Location { get; set; }
    [Required, Range(1,3)]
    public long CategoryTypeId { get; set; }
    [Required, Range(1,6)]
    public long ConditionTypeId { get; set; }

    public bool Delivery { get; set; }
    public bool Collection { get; set; }
    public bool Postage { get; set; }
    public bool Recover { get; set; }
}