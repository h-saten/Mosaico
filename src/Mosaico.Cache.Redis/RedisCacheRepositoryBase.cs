using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Mosaico.Cache.Base;
using Mosaico.Cache.Redis.Configurations;
using Mosaico.Infrastructure.Redis.Exceptions;
using Newtonsoft.Json;
using Serilog;
using StackExchange.Redis;

namespace Mosaico.Cache.Redis
{
    public abstract class RedisCacheRepositoryBase<T> : ICacheRepository<T> where T : CacheItemBase
    {
        protected readonly RedisCacheConfiguration Config;
        protected readonly IValidator<RedisCacheConfiguration> ConfigValidator;
        protected readonly ILogger Logger;

        protected virtual string EntityType => typeof(T).Name;
        
        protected RedisCacheRepositoryBase(RedisCacheConfiguration config, ILogger logger = null, IValidator<RedisCacheConfiguration> validator = null)
        {
            Config = config;
            Logger = logger;
            ConfigValidator = validator;
        }

        protected virtual HashEntry MapToHashEntry(T item)
        {
            if (string.IsNullOrWhiteSpace(item?.Name))
            {
                throw new InvalidRedisQueryParameterException("name");
            }
            return new HashEntry(item.Name, JsonConvert.SerializeObject(item));
        }

        protected virtual T MapToObject(RedisValue entry)
        {
            if (!string.IsNullOrWhiteSpace(entry.ToString()))
            {
                return JsonConvert.DeserializeObject<T>(entry.ToString());
            }

            return default;
        }
        
        protected virtual T MapToObject(HashEntry entry)
        {
            if (!string.IsNullOrWhiteSpace(entry.ToString()))
            {
                return JsonConvert.DeserializeObject<T>(entry.Value.ToString());
            }

            return default;
        }

        protected string GetCollectionName()
        {
            if (Config?.Mappings != null && Config.Mappings.TryGetValue(EntityType, out var newColName))
            {
                return newColName;
            }
            return EntityType;
        }
        
        protected virtual IConnectionMultiplexer GetConnection() {
            if (ConfigValidator != null)
            {
                var result = ConfigValidator.Validate(Config);
                if (!result.IsValid)
                {
                    var error = result.Errors.FirstOrDefault()?.ErrorMessage ?? "Redis is misconfigured";
                    Logger?.Error(error);
                    throw new ValidationException(error);
                }
            }
            return ConnectionMultiplexer.Connect(Config.RedisConnectionString);
        }
        
        public virtual async Task CreateAsync(T item, TimeSpan? expiryTime = null)
        {
            if (item == null)
            {
                throw new InvalidRedisQueryParameterException(nameof(item));
            }
            
            using (var connection = GetConnection())
            {
                var db = connection.GetDatabase(Config.RedisDatabase);
                var collectionName = GetCollectionName();
                Logger?.Verbose($"Creating new item {item.Name} in {collectionName}");
                var entry = MapToHashEntry(item);
                await db.HashSetAsync(collectionName, new []{ entry });
                if (expiryTime != null)
                {
                    Logger?.Verbose($"entry {collectionName} expires in {expiryTime.Value.Minutes}");
                    await db.KeyExpireAsync(collectionName, expiryTime);
                }
                Logger?.Verbose($"Item was successfully created");
            }
        }

        public virtual async Task CreateAsync(IEnumerable<T> items)
        {
            if (items != null && items.Any())
            {
                using (var connection = GetConnection())
                {
                    var db = connection.GetDatabase(Config.RedisDatabase);
                    var collectionName = GetCollectionName();
                    Logger?.Verbose($"Attempting to create {items.Count()} items in {collectionName}");
                    var entries = items.Select(MapToHashEntry);
                    await db.HashSetAsync(collectionName, entries.ToArray());
                    Logger?.Verbose("Items were successfully created");
                }
            }
        }

        public virtual async Task<T> GetAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidRedisQueryParameterException(nameof(name));
            }
            
            using (var connection = GetConnection())
            {
                var db = connection.GetDatabase(Config.RedisDatabase);
                var collectionName = GetCollectionName();
                Logger?.Verbose($"Attempting to retrieve an entity {name} from {collectionName}");
                var hashEntry = await db.HashGetAsync(collectionName, name);
                var item = MapToObject(hashEntry);
                if (item != null)
                {
                    Logger?.Verbose($"Entity {item.Name} retrieved");
                }
                else
                {
                    Logger?.Verbose($"Item was not found");
                }

                return item;
            }
        }
        
        public virtual async Task<ReadOnlyCollection<T>> GetAsync(IEnumerable<string> names)
        {
            if (names != null && names.Any())
            {
                using (var connection = GetConnection())
                {
                    var db = connection.GetDatabase(Config.RedisDatabase);
                    var collectionName = GetCollectionName();
                    Logger?.Verbose($"Attempting to retrieve multiple entities by names");
                    var hashEntries = await db.HashGetAsync(collectionName, names.Select(n => new RedisValue(n)).ToArray());
                    var items = hashEntries.Select(MapToObject).ToList();
                    Logger?.Verbose($"Successfully retrieved {items.Count()} entities");
                    return items.AsReadOnly();
                }
            }

            return new List<T>().AsReadOnly();
        }

        public virtual Task<ReadOnlyCollection<T>> GetAsync(int take = 10, int skip = 0)
        {
            if (take <= 0 || take > 1000)
            {
                throw new InvalidRedisQueryParameterException(nameof(take));
            }

            if (skip < 0)
            {
                throw new InvalidRedisQueryParameterException(nameof(skip));
            }
            
            using (var connection = GetConnection())
            {
                var db = connection.GetDatabase(Config.RedisDatabase);
                var collectionName = GetCollectionName();
                Logger?.Verbose($"Retrieveing multiple paginated entities from {collectionName}");
                var hashEntries = db.HashScan(collectionName, "*", take, 0, skip).Take(take);
                var items = hashEntries.Select(MapToObject).ToList();
                Logger?.Verbose($"Found {items.Count()} items");
                return Task.FromResult(items.AsReadOnly());
            }
        }

        public virtual async Task DeleteAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidRedisQueryParameterException(nameof(name));
            }
            
            using (var connection = GetConnection())
            {
                var db = connection.GetDatabase(Config.RedisDatabase);
                var collectionName = GetCollectionName();
                Logger?.Verbose($"Deleting {name} from {collectionName}");
                await db.HashDeleteAsync(collectionName, name);
                Logger?.Verbose($"Successfully deleted");
            }
        }

        public virtual Task DeleteAsync(T item)
        {
            if (item == null)
            {
                throw new InvalidRedisQueryParameterException(nameof(item));
            }

            return DeleteAsync(item.Name);
        }

        public virtual async Task DeleteAsync(IEnumerable<string> names)
        {
            if (names != null && names.Any())
            {
                using (var connection = GetConnection())
                {
                    var db = connection.GetDatabase(Config.RedisDatabase);
                    var collectionName = GetCollectionName();
                    Logger?.Verbose($"Deleting multiple entities from {collectionName}");
                    await db.HashDeleteAsync(collectionName, names.Select(n => new RedisValue(n)).ToArray());
                    Logger?.Verbose($"Successfully deleted");
                }
            }
        }

        public virtual Task DeleteAsync(IEnumerable<T> items)
        {
            if (items != null && items.Any())
            {
                return DeleteAsync(items.Select(i => i.Name));
            }
            return Task.CompletedTask;
        }
    }
}