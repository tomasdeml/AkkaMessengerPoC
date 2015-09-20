using Akka.Actor;

namespace AkkaMessenger.Runtime
{
    static class ActorContextExtensions
    {
        public static void PublishEvent(this IActorContext context, object ev)
        {
            context.System.EventStream.Publish(ev);
        }
    }
}
