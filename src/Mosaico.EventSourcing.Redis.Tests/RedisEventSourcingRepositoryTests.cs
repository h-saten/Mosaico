using System;
using System.Linq;
using System.Threading.Tasks;
using Mosaico.EventSourcing.Base;
using Mosaico.EventSourcing.Redis.Configurations;
using Mosaico.EventSourcing.Redis.Extensions;
using Mosaico.EventSourcing.Redis.Tests.Helpers;
using Mosaico.EventSourcing.Redis.Validators;
using Mosaico.Tests.Base;
using NUnit.Framework;
using StackExchange.Redis;

namespace Mosaico.EventSourcing.Redis.Tests
{
    public class RedisEventSourcingRepositoryTests : TestBase
    {
        private RedisEventSourcingConfiguration _config;

        [TearDown]
        public void TearDown()
        {
            var redis = ConnectionMultiplexer.Connect(_config.RedisConnectionString);
            var db = redis.GetDatabase(_config.RedisDatabase);
            // delete the stream
            db.KeyDelete(_config.StreamName);
        }

        [SetUp]
        public void Setup()
        {
            _config = GetSettings<RedisEventSourcingConfiguration>(RedisEventSourcingConfiguration.SectionName);
            _config.StreamName = Guid.NewGuid().ToString();
        }
        
        [Test]
        public async Task CreateEventTest()
        {
            //Arrange
            var repo = new RedisEventRepository(_config, null, new RedisEventSourcingConfigurationValidator());
            var factory = new DefaultSystemEventFactory();
            var systemEvent = factory.Create("unit-test","test-type",new EventTestPayload { Value = "wow" });
            
            //Act
            var messageId = await repo.CreateEventAsync(systemEvent);
            
            //Assert
            Assert.NotNull(messageId);
            
            var redis = ConnectionMultiplexer.Connect(_config.RedisConnectionString);
            var db = redis.GetDatabase(_config.RedisDatabase);
            var entry = await db.StreamReadAsync(_config.StreamName, 0);
            Assert.NotNull(entry);
            Assert.AreEqual(1, entry.Length);
            
            var dbSystemEvent = entry.First().Values.ToSystemEvent();
            Assert.AreEqual(dbSystemEvent.Id, systemEvent.Id);
            Assert.AreEqual(dbSystemEvent.Source, systemEvent.Source);
            Assert.AreEqual(dbSystemEvent.Type, systemEvent.Type);
            Assert.AreEqual(dbSystemEvent.BodyString, systemEvent.BodyString);
        }

        [Test]
        public async Task ReadEventByRedisIdTest()
        {
            //Arrange
            var repo = new RedisEventRepository(_config, null, new RedisEventSourcingConfigurationValidator());
            var factory = new DefaultSystemEventFactory();
            var systemEvent = factory.Create("unit-test","test-type",new EventTestPayload { Value = "wow" });
            var redis = ConnectionMultiplexer.Connect(_config.RedisConnectionString);
            var db = redis.GetDatabase(_config.RedisDatabase);
            
            //Act
            var messageId = await db.StreamAddAsync(_config.StreamName, systemEvent.ToNameValueEntry());
            var dbSystemEvent = await repo.GetEventAsync(messageId);
            
            //Assert
            Assert.NotNull(dbSystemEvent);
            Assert.AreEqual(dbSystemEvent.Id, systemEvent.Id);
            Assert.AreEqual(dbSystemEvent.Source, systemEvent.Source);
            Assert.AreEqual(dbSystemEvent.Type, systemEvent.Type);
            Assert.AreEqual(dbSystemEvent.BodyString, systemEvent.BodyString);
        }
        
        [Test]
        public async Task ReadEventByObjectIdTest()
        {
            //Arrange
            var repo = new RedisEventRepository(_config, null, new RedisEventSourcingConfigurationValidator());
            var id = Guid.NewGuid();
            var factory = new DefaultSystemEventFactory();
            var systemEvent = factory.Create("unit-test","test-type",new EventTestPayload { Value = "wow" });
            systemEvent.Id = id;
            var redis = ConnectionMultiplexer.Connect(_config.RedisConnectionString);
            var db = redis.GetDatabase(_config.RedisDatabase);
            
            //Act
            var messageId = await db.StreamAddAsync(_config.StreamName, systemEvent.ToNameValueEntry());
            var dbSystemEvent = await repo.GetEventAsync(id.ToString());
            
            //Assert
            Assert.NotNull(dbSystemEvent);
            Assert.AreEqual(dbSystemEvent.Id, systemEvent.Id);
            Assert.AreEqual(dbSystemEvent.Source, systemEvent.Source);
            Assert.AreEqual(dbSystemEvent.Type, systemEvent.Type);
            Assert.AreEqual(dbSystemEvent.BodyString, systemEvent.BodyString);
        }

        [Test]
        public async Task ReadNonExistingEventTest()
        {
            //Arrange
            var repo = new RedisEventRepository(_config, null, new RedisEventSourcingConfigurationValidator());
            var messageId = Guid.NewGuid().ToString();
            
            //Act
            var dbSystemEvent = await repo.GetEventAsync(messageId);
            
            //Assert
            Assert.IsNull(dbSystemEvent);
        }

        [Test]
        public async Task GetEventsWithinDateRangeTest()
        {
            //Arrange
            var repo = new RedisEventRepository(_config, null, new RedisEventSourcingConfigurationValidator());
            var factory = new DefaultSystemEventFactory();
            var systemEvent1 = factory.Create("unit-test","test-type",new EventTestPayload { Value = "wow" });
            var systemEvent2 = factory.Create("unit-test","test-type",new EventTestPayload { Value = "wow" });
            var systemEvent3 = factory.Create("unit-test","test-type",new EventTestPayload { Value = "wow" });
            var systemEvent4 = factory.Create("unit-test","test-type",new EventTestPayload { Value = "wow" });
            var systemEvent5 = factory.Create("unit-test","test-type",new EventTestPayload { Value = "wow" });
            systemEvent1.CreatedAt = DateTimeOffset.Parse("2021-08-10T01:00:00Z");
            systemEvent2.CreatedAt = DateTimeOffset.Parse("2021-08-10T05:00:00Z");
            systemEvent3.CreatedAt = DateTimeOffset.Parse("2021-08-10T09:00:00Z");
            systemEvent4.CreatedAt = DateTimeOffset.Parse("2021-12-10T09:00:00Z");
            systemEvent5.CreatedAt = DateTimeOffset.Parse("2021-11-10T07:00:00Z");
            await repo.CreateEventAsync(systemEvent1);
            await repo.CreateEventAsync(systemEvent2);
            await repo.CreateEventAsync(systemEvent3);
            await repo.CreateEventAsync(systemEvent4);
            await repo.CreateEventAsync(systemEvent5);
            
            //Act
            var events = await repo.GetEventsAsync(DateTimeOffset.Parse("2021-08-01T00:00:00Z"), DateTimeOffset.Parse("2021-09-01T00:00:00Z"));
            //Assert
            Assert.IsNotEmpty(events);
            Assert.AreEqual(3, events.Count);
        }
        
        [Test]
        public async Task GetEventsWithinDateRangeAndTypeTest()
        {
            //Arrange
            var repo = new RedisEventRepository(_config, null, new RedisEventSourcingConfigurationValidator());
            var factory = new DefaultSystemEventFactory();
            var systemEvent1 = factory.Create("unit-test","test-type",new EventTestPayload { Value = "wow" });
            var systemEvent2 = factory.Create("unit-test","test-type",new EventTestPayload { Value = "wow" });
            var systemEvent3 = factory.Create("unit-test","test-type",new EventTestPayload { Value = "wow" });
            var systemEvent4 = factory.Create("unit-test","another-type",new EventTestPayload { Value = "wow" });
            var systemEvent5 = factory.Create("unit-test","another-type",new EventTestPayload { Value = "wow" });
            systemEvent1.CreatedAt = DateTimeOffset.Parse("2021-08-10T01:00:00Z");
            systemEvent2.CreatedAt = DateTimeOffset.Parse("2021-08-10T05:00:00Z");
            systemEvent3.CreatedAt = DateTimeOffset.Parse("2021-08-10T09:00:00Z");
            systemEvent4.CreatedAt = DateTimeOffset.Parse("2021-12-10T09:00:00Z");
            systemEvent5.CreatedAt = DateTimeOffset.Parse("2021-11-10T07:00:00Z");
            await repo.CreateEventAsync(systemEvent1);
            await repo.CreateEventAsync(systemEvent2);
            await repo.CreateEventAsync(systemEvent3);
            await repo.CreateEventAsync(systemEvent4);
            await repo.CreateEventAsync(systemEvent5);
            
            //Act
            var events = await repo.GetEventsAsync("another-type", DateTimeOffset.Parse("2021-11-01T00:00:00Z"), DateTimeOffset.Parse("2021-12-01T00:00:00Z"));
            //Assert
            Assert.IsNotEmpty(events);
            Assert.AreEqual(1, events.Count);
        }
        
        [Test]
        public async Task GetAllEventsTest()
        {
            //Arrange
            var factory = new DefaultSystemEventFactory();
            var repo = new RedisEventRepository(_config, null, new RedisEventSourcingConfigurationValidator());
            for (int i = 0; i < 1050; i++)
            {
                var systemEvent = factory.Create("unit-test","test-type",new EventTestPayload { Value = "wow" });
                await repo.CreateEventAsync(systemEvent);
            }
            
            //Act
            var events = await repo.GetEventsAsync();
            //Assert
            Assert.IsNotEmpty(events);
            Assert.AreEqual(1050, events.Count);
        }
        
        [Test]
        public async Task GetEventsWithTypeTest()
        {
            //Arrange
            var repo = new RedisEventRepository(_config, null, new RedisEventSourcingConfigurationValidator());
            var factory = new DefaultSystemEventFactory();
            var systemEvent1 = factory.Create("unit-test","test-type",new EventTestPayload { Value = "wow" });
            var systemEvent2 = factory.Create("unit-test","test-type",new EventTestPayload { Value = "wow" });
            var systemEvent3 = factory.Create("unit-test","test-type",new EventTestPayload { Value = "wow" });
            var systemEvent4 = factory.Create("unit-test","another-type",new EventTestPayload { Value = "wow" });
            var systemEvent5 = factory.Create("unit-test","another-type",new EventTestPayload { Value = "wow" });
            await repo.CreateEventAsync(systemEvent1);
            await repo.CreateEventAsync(systemEvent2);
            await repo.CreateEventAsync(systemEvent3);
            await repo.CreateEventAsync(systemEvent4);
            await repo.CreateEventAsync(systemEvent5);
            
            //Act
            var events = await repo.GetEventsAsync("another-type");
            
            //Assert
            Assert.IsNotEmpty(events);
            Assert.AreEqual(2, events.Count);
        }
    }
}