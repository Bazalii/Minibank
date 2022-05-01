using System;

namespace MiniBank.Core.Domains.Providers;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}