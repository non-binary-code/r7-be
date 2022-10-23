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

        public async Task<Item?> GetItemByItemId(long itemId)
        {
            return await _itemRepository.GetItemByItemId(itemId);
        }

        public async Task<Item?> AddItem(NewItemRequest item)
        {
            return await _itemRepository.AddItem(item);
        }

        public async Task<bool> EditItem(long itemId, EditItemRequest editItemRequest)
        {
            var item = await _itemRepository.GetItemByItemId(itemId);
            if (item == null)
            {
                return false;
            }

            await _itemRepository.EditItem(itemId, editItemRequest);
            return true;
        }

        public async Task<bool> ArchiveItem(long itemId, ArchiveItemRequest archiveItemRequest)
        {
            var item = await _itemRepository.GetItemByItemId(itemId);
            if (item == null)
            {
                return false;
            }

            await _itemRepository.ArchiveItem(itemId, archiveItemRequest);
            return true;
        }
    }
}