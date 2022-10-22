using r7.Models;

namespace r7.Repositories
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetItems();
        Task<Item> GetItemByItemId(long itemId);
        Task<Item> AddItem(NewItemRequest item);
        Task EditItem(Item item);
    }
}