using System;

namespace Mosaico.Base.Tools
{
    public interface IDateTimeProvider
    {
        DateTimeOffset Now();
    }
    
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset Now()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}