using System;
using Akka.Actor;

namespace AkkaMessenger
{ 
    internal class Program
    {
        public const int PersonCount = 1000;

        private static void Main()
        {
            var system = ActorSystem.Create("messenger-system");
            var messenger = system.ActorOf(Props.Create<MessengerActor>(), "messenger");
            var emailFeeder = system.ActorOf(Props.Create<EmailFeeder>(), "email-feeder");

            messenger.Tell(new CreateEmailBatch
            {
                BatchId = Guid.NewGuid(),
                NumberOfEmails = 100,
                EmailFeeder = emailFeeder
            });

            Console.ReadLine();
        }
    }
}