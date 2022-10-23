namespace r7.Models;

public class EditItemRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public long CategoryTypeId { get; set; }
    public long ConditionTypeId { get; set; }
    public bool Delivery { get; set; }
    public bool Collection { get; set; }
    public bool Postage { get; set; }
    public bool Recover { get; set; }
    public string PictureUrl { get; set; }
    public string Location { get; set; }
}