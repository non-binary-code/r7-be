using r7.Models;
using Serilog;

namespace r7.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private IQuery _query;

        public ItemRepository(IQuery query)
        {
            _query = query;
        }

        public async Task<IEnumerable<Item>> GetItems()
        {
            var sqlStatement = GetAllItemsSqlStatement();
            var items = await _query.QueryAsync<Item>(sqlStatement);

            return items;
        }

        public async Task<Item> GetItemByItemId(long itemId)
        {
            var sql = GetItemByIdSqlStatement();
            var item = await _query.QueryFirstOrDefaultAsync<Item>(sql, new
            {
                ItemId = itemId
            });

            return item;
        }

        public async Task<Item> AddItem(NewItemRequest item)
        {
            var sql = AddItemSqlStatement();
            var id = await _query.ExecuteScalarAsync<long>(sql, item);

            return await GetItemByItemId(id);
        }

        public async Task EditItem(Item item)
        {
            var sql = EditItemSqlStatement();
            await _query.ExecuteAsync(sql, item);
        }

        private static string GetAllItemsSqlStatement()
        {
            return $@"SELECT Id, Name, Description, CategoryTypeId, ConditionTypeId, Delivery, Collection, Postage, Recover, PictureUrl, Location FROM items";
        }

        private static string GetItemByIdSqlStatement()
        {
            return $@"
            SELECT Id, Name, Description, CategoryTypeId, ConditionTypeId, Delivery, Collection, Postage, Recover, PictureUrl, Location FROM items
            WHERE id = @ItemId";
        }

        private static string AddItemSqlStatement()
        {
            return $@"INSERT INTO items
                 (
                   Name, Description, CategoryTypeId, ConditionTypeId, Delivery, Collection, Postage, Recover, PictureUrl, Location
                 )
                 VALUES
                 (
                   @Name, @Description, @CategoryTypeId, @ConditionTypeId, @Delivery, @Collection, @Postage, @Recover, @PictureUrl, @Location
                 ) RETURNING Id";
        }

        private static string EditItemSqlStatement()
        {
            return $@"UPDATE items
                    SET
                    Name = @Name,
                    Description = @Description,
                    CategoryTypeId = @CategoryTypeId,
                    ConditionTypeId = @ConditionTypeId,
                    Delivery = @Delivery,
                    Collection = @Collection,
                    Postage = @Postage,
                    Recover = @Recover,
                    PictureUrl = @PictureUrl,
                    Location = @Location
                    WHERE Id = @Id";
        }
    }
}