using r7.Models;

namespace r7.Services;

public interface IItemService
{
    Task<IEnumerable<ReuseItem>> GetItems(ReuseQueryParameters queryParameters);
    Task<IEnumerable<RecycleItem>> GetItems(RecycleQueryParameters queryParameters);
    Task<IEnumerable<RepairItem>> GetItems(RepairQueryParameters queryParameters);
    Task<Item?> GetItemByItemId(long itemId);
    Task<ReuseItem?> AddItem(NewReuseItemRequest newItem);
    Task<RecycleItem?> AddItem(NewRecycleItemRequest newItem);
    Task<RepairItem?> AddItem(NewRepairItemRequest newItem);
    Task<bool> EditItem(long itemId, EditItemRequest editItemRequest);
    Task<bool> ArchiveItem(long itemId, ArchiveItemRequest archiveItemRequest);
}