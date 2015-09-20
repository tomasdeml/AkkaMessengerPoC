using Akka.Actor;

namespace AkkaMessenger.Runtime.Jobs.Messages
{
    class GetJobReply
    {
        public IActorRef Job { get; private set; }

        public GetJobReply(IActorRef job)
        {
            Job = job;
        }
    }
}