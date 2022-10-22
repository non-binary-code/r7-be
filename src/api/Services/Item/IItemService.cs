using r7.Models;

namespace r7.Services;

public interface IItemService
{
    Task<IEnumerable<Item>> GetItems();
    Task<Item> GetItemByItemId(long itemId);
    Task<Item> AddItem(NewItemRequest user);
    Task EditItem(Item user);
}