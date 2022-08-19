namespace Mosaico.Infrastructure.Redis
{
    public static class Constants
    {
        public const int DefaultPageSize = 1000;

        public static class ErrorCodes
        {
            public const string InvalidRedisQueryParameters = "INVALID_REDIS_QUERY_PARAM";
            public const string RedisOperationFailed = "REDIS_OPERATION_FAILED";
        }
    }
}