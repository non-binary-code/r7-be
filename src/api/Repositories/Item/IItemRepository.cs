using r7.Models;

namespace r7.Repositories
{
    public interface IItemRepository
    {
        Task<IEnumerable<ReuseItem?>> GetItems(ReuseQueryParameters queryParameters);
        Task<IEnumerable<RecycleItem?>> GetItems(RecycleQueryParameters queryParameters);
        Task<IEnumerable<RepairItem?>> GetItems(RepairQueryParameters queryParameters);
        Task<Item?> GetItemByItemId(long itemId);
        Task<ReuseItem?> AddItem(NewReuseItemRequest item);
        Task<RecycleItem?> AddItem(NewRecycleItemRequest item);
        Task<RepairItem?> AddItem(NewRepairItemRequest item);
        Task EditItem(long itemId, EditItemRequest editItemRequest);
        Task ArchiveItem(long itemId, ArchiveItemRequest archiveItemRequest);
    }
}