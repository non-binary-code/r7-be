using r7.Models;

namespace r7.Services;

public interface IItemService
{
    Task<IEnumerable<Item>> GetItems(QueryParameters queryParameters);
    Task<Item?> GetItemByItemId(long itemId);
    Task<Item?> AddItem(NewReuseItemRequest newItem);
    Task<Item?> AddItem(NewRecycleItemRequest newItem);
    Task<Item?> AddItem(NewRepairItemRequest newItem);
    Task<bool> EditItem(long itemId, EditItemRequest editItemRequest);
    Task<bool> ArchiveItem(long itemId, ArchiveItemRequest archiveItemRequest);
}