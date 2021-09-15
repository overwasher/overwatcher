using System;

namespace Overwatcher.Services
{
    public class DateTimeProvider
    {
        public virtual DateTime Now => UtcNow.ToLocalTime();
        public virtual DateTime UtcNow => DateTime.UtcNow;
    }
}