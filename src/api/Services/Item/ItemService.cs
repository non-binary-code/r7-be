using r7.Models;
using r7.Repositories;

namespace r7.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<IEnumerable<Item>> GetItems(QueryParameters queryParameters)
        {
            return await _itemRepository.GetItems(queryParameters);
        }

        public async Task<Item> GetItemByItemId(long itemId)
        {
            return await _itemRepository.GetItemByItemId(itemId);
        }

        public async Task<Item> AddItem(NewItemRequest item)
        {
            return await _itemRepository.AddItem(item);
        }

        public async Task EditItem(Item user)
        {
            await _itemRepository.EditItem(user);
        }
    }
}