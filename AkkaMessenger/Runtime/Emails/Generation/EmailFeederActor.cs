using System;
using System.Linq;
using Akka.Actor;

namespace AkkaMessenger.Runtime.Emails.Generation
{
    internal class EmailFeederActor : ReceiveActor
    {
        public EmailFeederActor()
        {
            Receive(new Action<FeedEmailsRequest>(OnFeedEmailsRequest));
        }

        static void OnFeedEmailsRequest(FeedEmailsRequest message)
        {
            Console.WriteLine($"Feeding emails for split {message.SplitId}");

            Context.Sender.Tell(new FeedEmailsReply(message,
                Enumerable.Repeat(new Email("hop@gmail.com", "Hello"), message.ToId - message.FromId + 1).ToList()));
        }
    }
}