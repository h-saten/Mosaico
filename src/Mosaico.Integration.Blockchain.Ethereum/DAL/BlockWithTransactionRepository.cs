using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Cache.Redis.Configurations;
using Mosaico.Infrastructure.Redis.Exceptions;
using Newtonsoft.Json;
using Serilog;
using StackExchange.Redis;

namespace Mosaico.Integration.Blockchain.Ethereum.DAL
{
    public class BlockWithTransactionRepository : IBlockWithTransactionRepository
    {
        protected readonly RedisCacheConfiguration Config;
        protected readonly IValidator<RedisCacheConfiguration> ConfigValidator;
        protected readonly ILogger Logger;

        public BlockWithTransactionRepository(RedisCacheConfiguration config, IValidator<RedisCacheConfiguration> configValidator = null, ILogger logger = null)
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
        
        public virtual async Task AddAsync<T>(string blockchain, string blockHash, T item)
        {
            if (item == null)
            {
                throw new InvalidRedisQueryParameterException(nameof(item));
            }
            
            using (var connection = GetConnection())
            {
                var db = connection.GetDatabase(Config.RedisDatabase);
                Logger?.Verbose($"Creating new {blockchain} block with hash: '{blockHash}'.");
                var redisValue = MapToRedisValue(item);
                await db.HashSetAsync($"{blockchain}_{blockHash}", "block", redisValue);
                Logger?.Verbose($"Item was successfully created");
            }
        }

        public virtual async Task<T> GetAsync<T>(string blockchain, string blockHash)
        {
            using var connection = GetConnection();
            var db = connection.GetDatabase(Config.RedisDatabase);
            var response = await db.HashGetAsync($"{blockchain}_{blockHash}", "block");
            return MapToObject<T>(response);
        }
    }
}