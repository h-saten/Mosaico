using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mosaico.Cache.Redis.Configurations;
using Mosaico.Cache.Redis.Tests.Helpers;
using Mosaico.Cache.Redis.Validators;
using Mosaico.Infrastructure.Redis;
using Mosaico.Infrastructure.Redis.Exceptions;
using Mosaico.Infrastructure.Redis.Tests.Helpers;
using Mosaico.Tests.Base;
using Newtonsoft.Json;
using NUnit.Framework;
using StackExchange.Redis;

namespace Mosaico.Cache.Redis.Tests
{
    public class RedisCacheRepositoryTests : TestBase
    {
        private RedisCacheConfiguration _config;
        
        [TearDown]
        public async Task TearDown()
        {
            var redis = await ConnectionMultiplexer.ConnectAsync(_config.RedisConnectionString);
            var db = redis.GetDatabase(_config.RedisDatabase);
            // delete the stream
            db.KeyDelete(Constants.RedisCacheTestTypeName);
        }

        [SetUp]
        public void Setup()
        {
            _config = GetSettings<RedisCacheConfiguration>(RedisCacheConfiguration.SectionName);
        }

        [Test]
        public async Task CreateItemTest()
        {
            //Arrange
            var now = DateTimeOffset.Now;
            var itemName = "page_name";
            var item = new TestCacheItem
            {
                Name = itemName,
                CreatedAt = now
            };
            var repo = new RedisCacheTestRepository(_config, null, new RedisCacheConfigurationValidator());
            
            //Act
            await repo.CreateAsync(item);
            
            //Assert
            var redis = ConnectionMultiplexer.Connect(_config.RedisConnectionString);
            var db = redis.GetDatabase(_config.RedisDatabase);
            var hash = db.HashGet(Constants.RedisCacheTestTypeName, itemName);
            var dbItem = JsonConvert.DeserializeObject<TestCacheItem>(hash.ToString());
            Assert.NotNull(dbItem);
            Assert.AreEqual(now.ToString(), dbItem.CreatedAt.ToString());
        }
        
        [Test]
        public void FailCreateItemWithoutNameTest()
        {
            //Arrange
            var item = new TestCacheItem
            {
                CreatedAt = DateTimeOffset.Now
            };
            var repo = new RedisCacheTestRepository(_config, null, new RedisCacheConfigurationValidator());
            
            //Act
            //Assert
            Assert.ThrowsAsync<InvalidRedisQueryParameterException>(async () => await repo.CreateAsync(item));
        }
        
        [Test]
        public async Task CreateMultipleItemsTest()
        {
            //Arrange
            var now = DateTimeOffset.Now;
            var items = new List<TestCacheItem>();
            for (int i = 0; i < 50; i++)
            {
                items.Add(new TestCacheItem
                {
                    Name = Guid.NewGuid().ToString(),
                    CreatedAt = DateTimeOffset.Now
                });
            }
            var repo = new RedisCacheTestRepository(_config, null, new RedisCacheConfigurationValidator());
            
            //Act
            await repo.CreateAsync(items);
            
            //Assert
            var redis = ConnectionMultiplexer.Connect(_config.RedisConnectionString);
            var db = redis.GetDatabase(_config.RedisDatabase);
            var hashLength = db.HashLength(Constants.RedisCacheTestTypeName);
            Assert.AreEqual(50, hashLength);
        }

        [Test]
        public async Task GetItemTest()
        {
            //Arrange
            var now = DateTimeOffset.Now;
            var item = new TestCacheItem
            {
                Name = "page_name",
                CreatedAt = now
            };
            var item2 = new TestCacheItem
            {
                Name = "page_name2",
                CreatedAt = now
            };
            var redis = ConnectionMultiplexer.Connect(_config.RedisConnectionString);
            var db = redis.GetDatabase(_config.RedisDatabase);
            await db.HashSetAsync(Constants.RedisCacheTestTypeName, new []{ new HashEntry(item.Name, JsonConvert.SerializeObject(item)), new HashEntry(item2.Name, JsonConvert.SerializeObject(item2)) });
            var repo = new RedisCacheTestRepository(_config, null, new RedisCacheConfigurationValidator());

            //Act
            var dbItem = await repo.GetAsync("page_name");
            
            //Assert
            Assert.NotNull(dbItem);
            Assert.AreEqual(now, dbItem.CreatedAt);
            Assert.AreEqual("page_name", dbItem.Name);
        }
        
        [Test]
        public async Task GetNonExistingItemTest()
        {
            //Arrange
            var repo = new RedisCacheTestRepository(_config, null, new RedisCacheConfigurationValidator());
            var redis = ConnectionMultiplexer.Connect(_config.RedisConnectionString);
            var db = redis.GetDatabase(_config.RedisDatabase);
            
            //Act
            var exists = await db.HashExistsAsync(Constants.RedisCacheTestTypeName, "page_name");
            Assert.IsFalse(exists);
            var dbItem = await repo.GetAsync("page_name");
            
            //Assert
            Assert.IsNull(dbItem);
        }
        
        [Test]
        public async Task GetManyItemsTest()
        {
            //Arrange
            var now = DateTimeOffset.Now;
            var item = new TestCacheItem
            {
                Name = "page_name",
                CreatedAt = now
            };
            var item2 = new TestCacheItem
            {
                Name = "page_name2",
                CreatedAt = now
            };
            var item3 = new TestCacheItem
            {
                Name = "page_name3",
                CreatedAt = now
            };
            var redis = ConnectionMultiplexer.Connect(_config.RedisConnectionString);
            var db = redis.GetDatabase(_config.RedisDatabase);
            await db.HashSetAsync(Constants.RedisCacheTestTypeName, new []
            {
                new HashEntry(item.Name, JsonConvert.SerializeObject(item)), 
                new HashEntry(item2.Name, JsonConvert.SerializeObject(item2)),
                new HashEntry(item3.Name, JsonConvert.SerializeObject(item3))
            });
            var repo = new RedisCacheTestRepository(_config, null, new RedisCacheConfigurationValidator());

            //Act
            var dbItems = await repo.GetAsync(new []{ "page_name", "page_name2" });
            
            //Assert
            Assert.NotNull(dbItems);
            Assert.AreEqual(2, dbItems.Count);
        }
        
        [Test]
        public async Task GetPaginatedItemsTest()
        {
            //Arrange
            var items = new List<TestCacheItem>();
            
            for (int i = 0; i < 50; i++)
            {
                items.Add(new TestCacheItem
                {
                    Name = $"{i + 1}",
                    CreatedAt = DateTimeOffset.Now
                });
            }
            
            var redis = ConnectionMultiplexer.Connect(_config.RedisConnectionString);
            var db = redis.GetDatabase(_config.RedisDatabase);
            await db.HashSetAsync(Constants.RedisCacheTestTypeName, items.Select(i => new HashEntry(i.Name, JsonConvert.SerializeObject(i))).ToArray());
            var repo = new RedisCacheTestRepository(_config, null, new RedisCacheConfigurationValidator());

            //Act
            var dbItems = await repo.GetAsync(15, 10);
            
            //Assert
            Assert.NotNull(dbItems);
            Assert.AreEqual(15, dbItems.Count);
            Assert.AreEqual("11", dbItems[0].Name);
        }

        [Test]
        public async Task DeleteItemTest()
        {
            //Arrange
            var now = DateTimeOffset.Now;
            var item = new TestCacheItem
            {
                Name = "page_name",
                CreatedAt = now
            };
            var item2 = new TestCacheItem
            {
                Name = "page_name2",
                CreatedAt = now
            };
            var redis = ConnectionMultiplexer.Connect(_config.RedisConnectionString);
            var db = redis.GetDatabase(_config.RedisDatabase);
            await db.HashSetAsync(Constants.RedisCacheTestTypeName, new []{ new HashEntry(item.Name, JsonConvert.SerializeObject(item)), new HashEntry(item2.Name, JsonConvert.SerializeObject(item2)) });
            var repo = new RedisCacheTestRepository(_config, null, new RedisCacheConfigurationValidator());

            //Act
            await repo.DeleteAsync("page_name");
            
            //Assert
            var hashEntry = await db.HashGetAsync(Constants.RedisCacheTestTypeName, "page_name");
            var exists = await db.HashExistsAsync(Constants.RedisCacheTestTypeName, "page_name");
            var secondItemExists = await db.HashExistsAsync(Constants.RedisCacheTestTypeName, "page_name2");
            Assert.IsTrue(string.IsNullOrWhiteSpace(hashEntry));
            Assert.IsFalse(exists);
            Assert.IsTrue(secondItemExists);
        }
        
        [Test]
        public async Task DeleteItemByObjectTest()
        {
            //Arrange
            var now = DateTimeOffset.Now;
            var item = new TestCacheItem
            {
                Name = "page_name",
                CreatedAt = now
            };
            var item2 = new TestCacheItem
            {
                Name = "page_name2",
                CreatedAt = now
            };
            var redis = ConnectionMultiplexer.Connect(_config.RedisConnectionString);
            var db = redis.GetDatabase(_config.RedisDatabase);
            await db.HashSetAsync(Constants.RedisCacheTestTypeName, new []{ new HashEntry(item.Name, JsonConvert.SerializeObject(item)), new HashEntry(item2.Name, JsonConvert.SerializeObject(item2)) });
            var repo = new RedisCacheTestRepository(_config, null, new RedisCacheConfigurationValidator());

            //Act
            await repo.DeleteAsync(item2);
            
            //Assert
            var hashEntry = await db.HashGetAsync(Constants.RedisCacheTestTypeName, "page_name2");
            var exists = await db.HashExistsAsync(Constants.RedisCacheTestTypeName, "page_name2");
            var secondItemExists = await db.HashExistsAsync(Constants.RedisCacheTestTypeName, "page_name");
            Assert.IsTrue(string.IsNullOrWhiteSpace(hashEntry));
            Assert.IsFalse(exists);
            Assert.IsTrue(secondItemExists);
        }
        
        [Test]
        public async Task DeleteNonExistingItem()
        {
            //Arrange
            var redis = ConnectionMultiplexer.Connect(_config.RedisConnectionString);
            var db = redis.GetDatabase(_config.RedisDatabase);
            var repo = new RedisCacheTestRepository(_config, null, new RedisCacheConfigurationValidator());

            //Act
            var exists = await db.HashExistsAsync(Constants.RedisCacheTestTypeName, "page_name");
            Assert.IsFalse(exists);
            
            await repo.DeleteAsync("page_name");
            
            //Assert
            exists = await db.HashExistsAsync(Constants.RedisCacheTestTypeName, "page_name");
            Assert.IsFalse(exists);
        }
        
        [Test]
        public async Task DeleteManyItemsTest()
        {
            //Arrange
            var now = DateTimeOffset.Now;
            var item = new TestCacheItem
            {
                Name = "page_name",
                CreatedAt = now
            };
            var item2 = new TestCacheItem
            {
                Name = "page_name2",
                CreatedAt = now
            };
            var redis = ConnectionMultiplexer.Connect(_config.RedisConnectionString);
            var db = redis.GetDatabase(_config.RedisDatabase);
            await db.HashSetAsync(Constants.RedisCacheTestTypeName, new []{ new HashEntry(item.Name, JsonConvert.SerializeObject(item)), new HashEntry(item2.Name, JsonConvert.SerializeObject(item2)) });
            var repo = new RedisCacheTestRepository(_config, null, new RedisCacheConfigurationValidator());

            //Act
            await repo.DeleteAsync(new []{ "page_name", "page_name2" });
            
            //Assert
            var entity1Exists = await db.HashExistsAsync(Constants.RedisCacheTestTypeName, "page_name");
            var entity2Exists = await db.HashExistsAsync(Constants.RedisCacheTestTypeName, "page_name2");
            Assert.IsFalse(entity1Exists);
            Assert.IsFalse(entity2Exists);
        }
        
        [Test]
        public async Task DeleteManyItemsByObjectTest()
        {
            //Arrange
            var now = DateTimeOffset.Now;
            var item = new TestCacheItem
            {
                Name = "page_name",
                CreatedAt = now
            };
            var item2 = new TestCacheItem
            {
                Name = "page_name2",
                CreatedAt = now
            };
            var redis = ConnectionMultiplexer.Connect(_config.RedisConnectionString);
            var db = redis.GetDatabase(_config.RedisDatabase);
            await db.HashSetAsync(Constants.RedisCacheTestTypeName, new []{ new HashEntry(item.Name, JsonConvert.SerializeObject(item)), new HashEntry(item2.Name, JsonConvert.SerializeObject(item2)) });
            var repo = new RedisCacheTestRepository(_config, null, new RedisCacheConfigurationValidator());

            //Act
            await repo.DeleteAsync(new []{ item, item2 });
            
            //Assert
            var entity1Exists = await db.HashExistsAsync(Constants.RedisCacheTestTypeName, "page_name");
            var entity2Exists = await db.HashExistsAsync(Constants.RedisCacheTestTypeName, "page_name2");
            Assert.IsFalse(entity1Exists);
            Assert.IsFalse(entity2Exists);
        }
    }
}