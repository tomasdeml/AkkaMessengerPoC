using System;

namespace AkkaMessenger.Runtime.Emails
{
    internal interface IBatchBound
    {
        Guid BatchId { get; }
    }
}