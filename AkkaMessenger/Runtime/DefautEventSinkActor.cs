using System;
using Akka.Actor;
using Akka.Event;

namespace AkkaMessenger.Runtime
{
    class DefautEventSinkActor : ReceiveActor
    {
        public DefautEventSinkActor()
        {
            Receive<LogEvent>(_ => {});
            ReceiveAny(e =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("EVENT {0}: {1}", e.GetType().Name, e);
                Console.ResetColor();
            });
        }
    }
}
