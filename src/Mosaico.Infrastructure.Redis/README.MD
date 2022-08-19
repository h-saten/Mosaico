# Event Sourcing with Redis

Redis is in-memory NoSQL database that allows performance improvements in applications that demand extensive data reads.
Event Sourcing comes hand-in-hand with CQRS. It allows to restore current database state by evaluating all transactions
in the pipe. In Redis 5.0 there is a new data
type [Stream](https://stackexchange.github.io/StackExchange.Redis/Streams.html) which perfectly fits the event sourcing
scenario. It can stream events to the subscribers (if required)
and persist those events.

# Using Event Sourcing in Application

Whenever an important event in the system occures, developer should create an event listener which will deserialize and
push event to redis.

Configure Redis in appsettings.json by adding section

```json
"EventSourcing": {
  "RedisConnectionString": "localhost:6379",
  "RedisDatabase": -1,
  "StreamName": "system_events"
}
```

Update Startup.cs to initialize Redis in DI (using `services.AddRedisEventSourcing(config)`
or `builder.RegisterModule(new RedisEventSourcingModule(config))`)

# Object Cache with Redis

Redis continues to serve as a great extension for CQRS pattern. Read storage can be fully implemented using Redis
HashSets and store different types of documents. Relational schema might be too complex to calculate specifically for
frontend. That's why it is required to use redis to store calculated views and serve through Querying layer.

To start using it, configure Redis in appsettings.json by adding section

```json
"Cache": {
  "RedisConnectionString": "localhost:6379",
  "RedisDatabase": -1,
  "Mappings": {
       //mapping between EntityTypes and Redis collection key
   }      
}
```

And register it in DI using `services.AddRedisObjectCache(config)`
or `builder.RegisterModule(new RedisCacheModule(config))`.

Whenever you want to create new cached object, you need to create new entity derived from `CacheItemBase.cs`. Then
create a repository derived from `RedisCacheRepositoryBase` and override methods you need. Then register your repository
in DI.

# Running Redis locally

- Download [AnotherRedisDesktopManager](https://github.com/qishibo/AnotherRedisDesktopManager) if you want to to access
  redis database via UI
- Start redis instance using docker [official image](https://hub.docker.com/_/redis)
  ``docker run -d --name some-redis -p 6379:6379 redis``