using System;

namespace MiniBank.Core.Domains.Providers.Implementations;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}