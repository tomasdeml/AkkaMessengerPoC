using System;
using System.Collections.Generic;
using Akka.Actor;
using AkkaMessenger.Runtime.Emails.Generation;
using AkkaMessenger.Runtime.Emails.Sending;

namespace AkkaMessenger.Runtime.Emails
{
    class EmailBatchActor : ReceiveActor
    {
        private const int NumberOfEmailsInSplit = 500;

        private readonly IActorRef emailSender; 
        private Guid batchId;
        private int numberOfEmails;
        private int numberOfSplits;
        private ISet<string> pendingSplitIds;

        public EmailBatchActor(IActorRef emailSender)
        {
            this.emailSender = emailSender;
            Become(PreCreated);
        }

        protected override void PreStart()
        {
            base.PreStart();
            Console.WriteLine($"Batch spawned at {Self.Path}");
        }

        private void PreCreated()
        {
            Receive(new Action<CreateBatch>(OnCreateBatch));
            ReceiveAny(Unhandled);
        }

        private void OnCreateBatch(CreateBatch message)
        {
            Console.WriteLine($"Creating batch {message.BatchId}");

            batchId = message.BatchId;
            numberOfEmails = message.NumberOfEmails;
            numberOfSplits = numberOfEmails / NumberOfEmailsInSplit;
            pendingSplitIds = new HashSet<string>();

            Become(Created);
        }

        private void Created()
        {
            Console.WriteLine($"Batch {batchId} became created");

            Receive(new Action<StartBatch>(OnStartBatch));
            ReceiveAny(Unhandled);
        }

        private void OnStartBatch(StartBatch message)
        {
            Console.WriteLine($"Starting batch {batchId}...");

            for (int i = 0; i < numberOfSplits; i++)
            {
                var feedEmailsRequest = new FeedEmailsRequest(i * NumberOfEmailsInSplit, (i + 1) * NumberOfEmailsInSplit - 1);
                pendingSplitIds.Add(feedEmailsRequest.SplitId);

                message.EmailFeeder.Tell(feedEmailsRequest);
                Console.WriteLine($"Split {feedEmailsRequest.SplitId} of batch {batchId} requested");
            }

            Become(AwaitingEmailFeeds);
        }

        private void AwaitingEmailFeeds()
        {
            Console.WriteLine($"Batch {batchId} became awaiting feeds");

            Receive(new Action<FeedEmailsReply>(OnFeedEmailsReply));
            ReceiveAny(Unhandled);
        }

        private void OnFeedEmailsReply(FeedEmailsReply message)
        {
            Console.WriteLine($"Received feed for split {message.SplitId}");

            if (!pendingSplitIds.Contains(message.SplitId))
            {
                Console.WriteLine("Received feed was already processed");
                return;
            }

            foreach (var email in message.Emails)
                emailSender.Tell(email);

            pendingSplitIds.Remove(message.SplitId);

            if (pendingSplitIds.Count != 0)
                return;

            Context.Parent.Tell(new BatchSent(batchId));
            Become(BatchSent);
        }

        private void BatchSent()
        {
            Console.WriteLine($"Batch {batchId} became sent");
            ReceiveAny(Unhandled);
        }
    }
}