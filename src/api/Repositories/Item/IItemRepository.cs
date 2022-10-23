using r7.Models;

namespace r7.Repositories
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetItems(QueryParameters queryParameters);
        Task<Item?> GetItemByItemId(long itemId);
        Task<Item?> AddItem(NewItemRequest item);
        Task EditItem(long itemId, EditItemRequest editItemRequest);
        Task ArchiveItem(long itemId, ArchiveItemRequest archiveItemRequest);
    }
}