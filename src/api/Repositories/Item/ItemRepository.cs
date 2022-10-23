using System.Text;
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

        public async Task<IEnumerable<Item>> GetItems(QueryParameters queryParameters)
        {
            var sqlStatement = GetAllItemsSqlStatement();
            if (HasQuery(queryParameters))
            {
                var additionalSql = QueryToSql(queryParameters);
                sqlStatement += additionalSql;
            }

            var items = await _query.QueryAsync<Item>(sqlStatement);

            return items;
        }

        private string QueryToSql(QueryParameters queryParameters)
        {
            var additionSql = new StringBuilder(" WHERE ");
            var foundQueryParam = false;

            if (queryParameters.CategoryTypeId.HasValue)
            {
                foundQueryParam = true;
                additionSql.Append($"CategoryTypeId = {queryParameters.CategoryTypeId}");
            }

            if (queryParameters.ConditionTypeId.HasValue)
            {
                if (foundQueryParam)
                {
                    additionSql.Append(" AND ");
                }

                foundQueryParam = true;
                additionSql.Append($"ConditionTypeId = {queryParameters.ConditionTypeId}");
            }

            if (queryParameters.Delivery)
            {
                if (foundQueryParam)
                {
                    additionSql.Append(" AND ");
                }

                foundQueryParam = true;
                additionSql.Append($"Delivery IS TRUE");
            }

            if (queryParameters.Collection)
            {
                if (foundQueryParam)
                {
                    additionSql.Append(" AND ");
                }

                foundQueryParam = true;
                additionSql.Append($"Collection IS TRUE");
            }

            if (queryParameters.Postage)
            {
                if (foundQueryParam)
                {
                    additionSql.Append(" AND ");
                }

                foundQueryParam = true;
                additionSql.Append($"Postage IS TRUE");
            }

            if (queryParameters.Recover)
            {
                if (foundQueryParam)
                {
                    additionSql.Append(" AND ");
                }

                foundQueryParam = true;
                additionSql.Append($"Recover IS TRUE");
            }
            
            return additionSql.ToString();
        }


        private static bool HasQuery(QueryParameters queryParameters)
        {
            return (queryParameters.CategoryTypeId.HasValue ||
                    queryParameters.ConditionTypeId.HasValue ||
                    queryParameters.Delivery ||
                    queryParameters.Collection ||
                    queryParameters.Postage ||
                    queryParameters.Recover);
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
            return $@"SELECT Id, Name, Description, CategoryTypeId, ConditionTypeId, Delivery, Collection, Postage, Recover, PictureUrl, Location, CurrentUserId FROM items";
        }

        private static string GetItemByIdSqlStatement()
        {
            return $@"
            SELECT Id, Name, Description, CategoryTypeId, ConditionTypeId, Delivery, Collection, Postage, Recover, PictureUrl, Location, CurrentUserId FROM items
            WHERE id = @ItemId";
        }

        private static string AddItemSqlStatement()
        {
            return $@"INSERT INTO items
                 (
                   Name, Description, CategoryTypeId, ConditionTypeId, Delivery, Collection, Postage, Recover, PictureUrl, Location, CurrentUserId
                 )
                 VALUES
                 (
                   @Name, @Description, @CategoryTypeId, @ConditionTypeId, @Delivery, @Collection, @Postage, @Recover, @PictureUrl, @Location, @UserId
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
                    Location = @Location,
                    CurrentUserId = @CurrentUserId
                    WHERE Id = @Id";
        }
    }
}