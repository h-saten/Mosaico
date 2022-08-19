using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Cache.Redis.Configurations;
using Mosaico.Infrastructure.Redis.Exceptions;
using Newtonsoft.Json;
using Serilog;
using StackExchange.Redis;

namespace Mosaico.Cache.Redis
{
    public class RedisSortedRepository : ITimeSeriesRepository
    {
        protected readonly RedisCacheConfiguration Config;
        protected readonly IValidator<RedisCacheConfiguration> ConfigValidator;
        protected readonly ILogger Logger;

        public RedisSortedRepository(RedisCacheConfiguration config, IValidator<RedisCacheConfiguration> configValidator = null, ILogger logger = null)
        {
            Config = config;
            ConfigValidator = configValidator;
            Logger = logger;
        }
        
        protected virtual RedisValue MapToRedisValue<T>(T item)
        {
            return new RedisValue(JsonConvert.SerializeObject(item));
        }

        protected virtual T MapToObject<T>(RedisValue entry)
        {
            return !string.IsNullOrWhiteSpace(entry.ToString()) ? JsonConvert.DeserializeObject<T>(entry.ToString()) : default;
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
        
        public virtual async Task AddAsync<T>(string setName, double key, T item)
        {
            if (item == null)
            {
                throw new InvalidRedisQueryParameterException(nameof(item));
            }
            
            using (var connection = GetConnection())
            {
                var db = connection.GetDatabase(Config.RedisDatabase);
                Logger?.Verbose($"Creating new item {key} in {setName}");
                var redisValue = MapToRedisValue(item);
                await db.SortedSetAddAsync(setName, redisValue, key, CommandFlags.FireAndForget);
                Logger?.Verbose($"Item was successfully created");
            }
        }

        public virtual async Task<List<T>> GetAsync<T>(string setName, double min, double max)
        {
            using (var connection = GetConnection())
            {
                var db = connection.GetDatabase(Config.RedisDatabase);
                var response = await db.SortedSetRangeByScoreAsync(setName, min, max, Exclude.None, Order.Ascending);
                return response.Select(MapToObject<T>).ToList();
            }
        }
        
        public virtual async Task<T> GetLastOrDefaultAsync<T>(string setName)
        {
            using (var connection = GetConnection())
            {
                var db = connection.GetDatabase(Config.RedisDatabase);
                var min = DateTimeOffset.MinValue.ToUnixTimeSeconds();
                var max = DateTimeOffset.MaxValue.ToUnixTimeSeconds();
                var response = await db.SortedSetRangeByScoreAsync(setName, min, max, Exclude.None, Order.Descending, skip: 0, take: 1);
                if (response != null && response.Any())
                {
                    var item = response.FirstOrDefault();
                    if (item != default)
                    {
                        return MapToObject<T>(item);
                    }
                }

                return default;
            }
        }
    }
}