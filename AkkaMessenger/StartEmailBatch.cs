using Akka.Actor;

namespace AkkaMessenger
{
    class StartEmailBatch
    {
        public IActorRef EmailFeeder { get; set; }

        public StartEmailBatch(IActorRef emailFeeder)
        {
            EmailFeeder = emailFeeder;
        }
    }
}