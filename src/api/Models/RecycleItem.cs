namespace r7.Models;

public class RecycleItem
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

    // Recycle
    public string Dimensions { get; set; }
    public decimal Weight { get; set; }
    public bool Compostable { get; set; }
    public string RecycleLocation { get; set; }
    public decimal Distance { get; set; }
}