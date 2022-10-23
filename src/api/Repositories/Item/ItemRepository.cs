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

        public async Task<IEnumerable<ReuseItem?>> GetItems(ReuseQueryParameters queryParameters)
        {
            var sqlStatement = GetAllItemsSqlStatement();

            if (HasQuery(queryParameters))
            {
                var additionalSql = QueryToSql(queryParameters);
                sqlStatement += additionalSql;
            }

            var items = await _query.QueryAsync<Item>(sqlStatement);

            return items.Select(ConvertToReuse);
        }

        private static ReuseItem? ConvertToReuse(Item? arg)
        {
            if (arg == null) return null;

            return new ReuseItem
            {
                Id = arg.Id,
                CurrentUserId = arg.CurrentUserId,
                Name = arg.Name,
                Description = arg.Description,
                PictureUrl = arg.PictureUrl,
                Location = arg.Location,
                Archived = arg.Archived,
                ArchivedReason = arg.ArchivedReason,
                CategoryTypeId = arg.CategoryTypeId,
                ConditionTypeId = arg.ConditionTypeId,
                Delivery = arg.Delivery,
                Collection = arg.Collection,
                Postage = arg.Postage,
                Recover = arg.Recover
            };
        }

        private static bool HasQuery(ReuseQueryParameters queryParameters)
        {
            return (queryParameters.CategoryTypeId.HasValue ||
                    queryParameters.ConditionTypeId.HasValue ||
                    queryParameters.Delivery ||
                    queryParameters.Collection ||
                    queryParameters.Postage ||
                    queryParameters.Recover ||
                    !queryParameters.IncludeArchived);
        }

        private static string QueryToSql(ReuseQueryParameters queryParameters)
        {
            var additionSql = new StringBuilder(" WHERE ItemTypeId = 1");

            if (!queryParameters.IncludeArchived)
            {
                additionSql.Append($" AND Archived IS FALSE");
            }

            if (queryParameters.CategoryTypeId.HasValue)
            {
                additionSql.Append($" AND CategoryTypeId = {queryParameters.CategoryTypeId}");
            }

            if (queryParameters.ConditionTypeId.HasValue)
            {
                additionSql.Append($" AND ConditionTypeId = {queryParameters.ConditionTypeId}");
            }

            if (queryParameters.Delivery)
            {
                additionSql.Append($" AND Delivery IS TRUE");
            }

            if (queryParameters.Collection)
            {
                additionSql.Append($" AND Collection IS TRUE");
            }

            if (queryParameters.Postage)
            {
                additionSql.Append($" AND Postage IS TRUE");
            }

            if (queryParameters.Recover)
            {
                additionSql.Append($" AND Recover IS TRUE");
            }

            additionSql.Append(" order by ID DESC");

            return additionSql.ToString();
        }

        public async Task<IEnumerable<RecycleItem?>> GetItems(RecycleQueryParameters queryParameters)
        {
            var sqlStatement = GetAllItemsSqlStatement();
            if (HasQuery(queryParameters))
            {
                var additionalSql = QueryToSql(queryParameters);
                sqlStatement += additionalSql;
            }

            var items = await _query.QueryAsync<Item>(sqlStatement);

            return items.Select(ConvertToRecycle);
        }

        private static RecycleItem? ConvertToRecycle(Item? arg)
        {
            if (arg == null) return null;

            return new RecycleItem
            {
                Id = arg.Id,
                CurrentUserId = arg.CurrentUserId,
                Name = arg.Name,
                Description = arg.Description,
                PictureUrl = arg.PictureUrl,
                Location = arg.Location,
                Archived = arg.Archived,
                ArchivedReason = arg.ArchivedReason,
                Compostable = arg.Compostable,
                Dimensions = arg.Dimensions,
                Distance = arg.Distance,
                RecycleLocation = arg.RecycleLocation,
                Weight = arg.Weight
            };
        }

        private static bool HasQuery(RecycleQueryParameters queryParameters)
        {
            return (queryParameters.Compostable ||
                    !queryParameters.IncludeArchived);
        }

        private static string QueryToSql(RecycleQueryParameters queryParameters)
        {
            var additionSql = new StringBuilder(" WHERE ItemTypeId = 2");

            if (!queryParameters.IncludeArchived)
            {
                additionSql.Append($" AND Archived IS FALSE");
            }

            if (queryParameters.Compostable)
            {
                additionSql.Append($" AND Compostable IS TRUE");
            }

            additionSql.Append(" order by ID DESC");

            return additionSql.ToString();
        }

        public async Task<IEnumerable<RepairItem?>> GetItems(RepairQueryParameters queryParameters)
        {
            var sqlStatement = GetAllItemsSqlStatement();
            if (HasQuery(queryParameters))
            {
                var additionalSql = QueryToSql(queryParameters);
                sqlStatement += additionalSql;
            }

            var items = await _query.QueryAsync<Item>(sqlStatement);

            return items.Select(ConvertToRepair);
        }

        private static RepairItem? ConvertToRepair(Item? arg)
        {
            if (arg == null) return null;

            return new RepairItem
            {
                Id = arg.Id,
                CurrentUserId = arg.CurrentUserId,
                Name = arg.Name,
                Description = arg.Description,
                PictureUrl = arg.PictureUrl,
                Location = arg.Location,
                Archived = arg.Archived,
                ArchivedReason = arg.ArchivedReason
            };
        }

        private static bool HasQuery(RepairQueryParameters queryParameters)
        {
            return (!queryParameters.IncludeArchived);
        }

        private static string QueryToSql(RepairQueryParameters queryParameters)
        {
            var additionSql = new StringBuilder(" WHERE ItemTypeId = 3");

            if (!queryParameters.IncludeArchived)
            {
                additionSql.Append($" AND Archived IS FALSE");
            }

            additionSql.Append(" order by ID DESC");

            return additionSql.ToString();
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

        public async Task<ReuseItem?> AddItem(NewReuseItemRequest item)
        {
            var sql = AddReuseItemSqlStatement();
            var id = await _query.ExecuteScalarAsync<long>(sql, new
            {
                item.Name,
                item.UserId,
                item.Description,
                item.PictureUrl,
                item.Location,
                item.CategoryTypeId,
                item.ConditionTypeId,
                item.Delivery,
                item.Collection,
                item.Postage,
                item.Recover
            });

            return ConvertToReuse(await GetItemByItemId(id));
        }

        public async Task<RecycleItem?> AddItem(NewRecycleItemRequest item)
        {
            var sql = AddRecycleItemSqlStatement();
            var id = await _query.ExecuteScalarAsync<long>(sql, new
            {
                item.Name,
                item.UserId,
                item.Description,
                item.PictureUrl,
                item.Location,
                item.RecycleLocation,
                item.Distance,
                item.Weight,
                item.Dimensions,
                item.Compostable
            });

            return ConvertToRecycle(await GetItemByItemId(id));
        }

        public async Task<RepairItem?> AddItem(NewRepairItemRequest item)
        {
            var sql = AddRepairItemSqlStatement();
            var id = await _query.ExecuteScalarAsync<long>(sql, new
            {
                item.Name,
                item.UserId,
                item.Description,
                item.PictureUrl,
                item.Location
            });

            return ConvertToRepair(await GetItemByItemId(id));
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
            return "SELECT * FROM items";
        }

        private static string GetItemByIdSqlStatement()
        {
            return $@"SELECT * FROM items WHERE id = @ItemId";
        }

        private static string AddReuseItemSqlStatement()
        {
            return $@"INSERT INTO items
                 (
                    Name,
                    CurrentUserId,
                    Description,
                    PictureUrl,
                    Location,
                    CategoryTypeId,
                    ConditionTypeId,
                    Delivery,
                    Collection,
                    Postage,
                    Recover,
                    Archived,
                    ItemTypeId
                 )
                 VALUES
                 (
                    @Name,
                    @UserId,
                    @Description,
                    @PictureUrl,
                    @Location,
                    @CategoryTypeId,
                    @ConditionTypeId,
                    @Delivery,
                    @Collection,
                    @Postage,
                    @Recover,
                    False,
                    1
                 ) RETURNING Id";
        }

        private static string AddRecycleItemSqlStatement()
        {
            return $@"INSERT INTO items
                 (
                    Name,
                    CurrentUserId,
                    Description,
                    PictureUrl,
                    Location,
                    CategoryTypeId,
                    ConditionTypeId,
                    Dimensions,
                    Weight,
                    Compostable,
                    RecycleLocation,
                    Distance,
                    Archived,
                    ItemTypeId
                 )
                 VALUES
                 (
                    @Name,
                    @UserId,
                    @Description,
                    @PictureUrl,
                    @Location,
                    1,
                    1,
                    @Dimensions,
                    @Weight,
                    @Compostable,
                    @RecycleLocation,
                    @Distance,
                    False,
                    2
                 ) RETURNING Id";
        }

        private static string AddRepairItemSqlStatement()
        {
            return $@"INSERT INTO items
                 (
                    Name,
                    CurrentUserId,
                    Description,
                    PictureUrl,
                    Location,
                    CategoryTypeId,
                    ConditionTypeId,
                    Archived,
                    ItemTypeId
                 )
                 VALUES
                 (
                    @Name,
                    @UserId,
                    @Description,
                    @PictureUrl,
                    @Location,
                    1,
                    1,
                    False,
                    3
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