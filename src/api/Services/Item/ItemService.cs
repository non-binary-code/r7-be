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

        public async Task<IEnumerable<ReuseItem>> GetItems(ReuseQueryParameters queryParameters)
        {
            return await _itemRepository.GetItems(queryParameters);
        }

        public async Task<IEnumerable<RecycleItem>> GetItems(RecycleQueryParameters queryParameters)
        {
            return await _itemRepository.GetItems(queryParameters);
        }

        public async Task<IEnumerable<RepairItem>> GetItems(RepairQueryParameters queryParameters)
        {
            return await _itemRepository.GetItems(queryParameters);
        }

        public async Task<Item?> GetItemByItemId(long itemId)
        {
            return await _itemRepository.GetItemByItemId(itemId);
        }

        public async Task<ReuseItem?> AddItem(NewReuseItemRequest item)
        {
            return await _itemRepository.AddItem(item);
        }

        public async Task<RecycleItem?> AddItem(NewRecycleItemRequest item)
        {
            return await _itemRepository.AddItem(item);
        }

        public async Task<RepairItem?> AddItem(NewRepairItemRequest item)
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