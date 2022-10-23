namespace r7.Models;

public class RepairItem
{
    // Common
    public long Id { get; set; }
    public long CurrentUserId { get; set; }
    
    public string Name { get; set; }
    public string Description { get; set; }
    public string PictureUrl { get; set; }
    public string Location { get; set; }

    public bool Archived { get; set; }
    public string ArchivedReason { get; set; }

    public bool Delivery { get; set; }
    public bool Collection { get; set; }
    public bool Postage { get; set; }
    public bool Recover { get; set; }
}