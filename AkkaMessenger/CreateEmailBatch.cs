using System;
using Akka.Actor;

namespace AkkaMessenger
{
    class CreateEmailBatch
    {
        public Guid BatchId { get; set; }
        public int NumberOfEmails { get; set; } 
        public IActorRef EmailFeeder { get; set; } 
    }
}