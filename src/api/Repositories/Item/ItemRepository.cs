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

        private static string QueryToSql(QueryParameters queryParameters)
        {
            var additionSql = new StringBuilder(" WHERE ");
            var foundQueryParam = false;

            if (!queryParameters.IncludeArchived)
            {
                foundQueryParam = true;
                additionSql.Append($"Archived IS FALSE");
            }

            if (queryParameters.CategoryTypeId.HasValue)
            {
                if (foundQueryParam)
                {
                    additionSql.Append(" AND ");
                }

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
                    queryParameters.Recover || 
                    !queryParameters.IncludeArchived);
        }

        public async Task<Item?> GetItemByItemId(long itemId)
        {
            var sql = GetItemByIdSqlStatement();
            var item = await _query.QueryFirstOrDefaultAsync<Item>(sql, new
            {
                ItemId = itemId
            });

            return item;
        }

        public async Task<Item?> AddItem(NewReuseItemRequest item)
        {
            var sql = AddItemSqlStatement();
            var id = await _query.ExecuteScalarAsync<long>(sql, new
            {
                item.Name,
                item.Description,
                item.CategoryTypeId,
                item.ConditionTypeId,
                item.Delivery,
                item.Collection,
                item.Postage,
                item.Recover,
                item.Location,
                item.PictureUrl,
                item.UserId,
                Archived = false,
                ItemTypeId = 1
            });

            return await GetItemByItemId(id);
        }

        public async Task<Item?> AddItem(NewRecycleItemRequest item)
        {
            var sql = AddRecycleItemSqlStatement();
            var id = await _query.ExecuteScalarAsync<long>(sql, new
            {
                item.Name,
                item.Description,
                item.PictureUrl,
                item.Location,
                item.RecycleLocation,
                item.Distance,
                item.Weight,
                item.Dimensions,
                item.Compostable,
                item.UserId,
                Archived = false
            });

            return await GetItemByItemId(id);
        }

        public async Task<Item?> AddItem(NewRepairItemRequest item)
        {
            var sql = AddItemSqlStatement();
            var id = await _query.ExecuteScalarAsync<long>(sql, new
            {
                item.Name,
                item.Description,
                item.CategoryTypeId,
                item.ConditionTypeId,
                item.Delivery,
                item.Collection,
                item.Postage,
                item.Recover,
                item.Location,
                item.PictureUrl,
                item.UserId,
                Archived = false,
                ItemTypeId = 3
            });

            return await GetItemByItemId(id);
        }
        
        public async Task EditItem(long itemId, EditItemRequest editItemRequest)
        {
            var sql = EditItemSqlStatement();
            var parameters = new
            {
                Id = itemId,
                editItemRequest.Name,
                editItemRequest.Description,
                editItemRequest.CategoryTypeId,
                editItemRequest.ConditionTypeId,
                editItemRequest.Delivery,
                editItemRequest.Collection,
                editItemRequest.Postage,
                editItemRequest.Recover,
                editItemRequest.PictureUrl,
                editItemRequest.Location
            };

            await _query.ExecuteAsync(sql, parameters);
        }

        public async Task ArchiveItem(long itemId, ArchiveItemRequest archiveItemRequest)
        {
            var sql = ArchiveSqlStatement();
            var parameters = new {Id = itemId, Archived = true, archiveItemRequest.ArchivedReason};
            await _query.ExecuteAsync(sql, parameters);
        }

        private static string GetAllItemsSqlStatement()
        {
            return $@"SELECT Id, Name, Description, CategoryTypeId, ConditionTypeId, Delivery, Collection, Postage, Recover, PictureUrl, Location, CurrentUserId, Archived, ArchivedReason FROM items";
        }

        private static string GetItemByIdSqlStatement()
        {
            return $@"
            SELECT Id, Name, Description, CategoryTypeId, ConditionTypeId, Delivery, Collection, Postage, Recover, PictureUrl, Location, CurrentUserId, Archived, ArchivedReason FROM items
            WHERE id = @ItemId";
        }

        private static string AddItemSqlStatement()
        {
            return $@"INSERT INTO items
                 (
                   Name, Description, CategoryTypeId, ConditionTypeId, Delivery, Collection, Postage, Recover, PictureUrl, Location, CurrentUserId, Archived, ItemTypeId
                 )
                 VALUES
                 (
                   @Name, @Description, @CategoryTypeId, @ConditionTypeId, @Delivery, @Collection, @Postage, @Recover, @PictureUrl, @Location, @UserId, @Archived, @ItemTypeId
                 ) RETURNING Id";
        }

        private static string AddRecycleItemSqlStatement()
        {
            return $@"INSERT INTO items
                 (
                    Name,
                    Description,
                    CategoryTypeId,
                    ConditionTypeId,
                    PictureUrl,
                    Location,
                    CurrentUserId,
                    Archived,
                    ItemTypeId,
                    RecycleLocation,
                    Distance,
                    Weight,
                    Dimensions,
                    Compostable
                 )
                 VALUES
                 (
                    @Name,
                    @Description,
                    1,
                    1,
                    @PictureUrl,
                    @Location,
                    @UserId,
                    @Archived,
                    2,
                    @RecycleLocation,
                    @Distance,
                    @Weight,
                    @Dimensions,
                    @Compostable
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

        private static string ArchiveSqlStatement()
        {
            return $@"UPDATE items
                    SET
                    Archived = @Archived,
                    ArchivedReason = @ArchivedReason
                    WHERE Id = @Id";
        }
    }
}