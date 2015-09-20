using System;
using Akka.Actor;

namespace AkkaMessenger.Runtime.Emails.Sending
{
    class StartBatch : IBatchBound
    {
        public Guid BatchId { get; set; }
        public IActorRef EmailFeeder { get; set; }

        public StartBatch(Guid batchId, IActorRef emailFeeder)
        {
            BatchId = batchId;
            EmailFeeder = emailFeeder;
        }
    }
}