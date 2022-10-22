using System.ComponentModel.DataAnnotations.Schema;

namespace r7.Models;

public class NewItemRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    [Column("CategoryType_Id")]
    public long CategoryTypeId { get; set; }
    [Column("ConditionType_Id")]
    public long ConditionTypeId { get; set; }
    public bool Delivery { get; set; }
    public bool Collection { get; set; }
    public bool Postage { get; set; }
    public bool Recover { get; set; }
    public string PictureUrl { get; set; }
    public string Location { get; set; }
}