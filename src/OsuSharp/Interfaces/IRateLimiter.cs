﻿using System.Threading;
using System.Threading.Tasks;

namespace OsuSharp.Interfaces
{
    public interface IRateLimiter
    {
        Task HandleAsync(CancellationToken cancellationToken = default);
    }
}