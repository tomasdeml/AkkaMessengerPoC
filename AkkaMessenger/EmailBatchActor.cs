using System;
using System.Collections.Generic;
using Akka.Actor;

namespace AkkaMessenger
{
    class EmailBatchActor : ReceiveActor
    {
        private const int NumberOfEmailsInSplit = 5;

        private readonly IActorRef emailSender;

        private Guid batchId;
        private int numberOfEmails;
        private int numberOfSplits;
        private IList<Guid> pendingSplitIds;

        public EmailBatchActor(IActorRef emailSender)
        {
            this.emailSender = emailSender;
            Become(Uninitialized);
        }

        private void Uninitialized()
        {
            Receive(new Action<PrepareBatch>(OnPrepareEmailBatch));
            ReceiveAny(Unhandled);
        }

        private void OnPrepareEmailBatch(PrepareBatch message)
        {
            batchId = message.BatchId;
            numberOfEmails = message.NumberOfEmails;
            numberOfSplits = numberOfEmails / NumberOfEmailsInSplit;
            pendingSplitIds = new List<Guid>(numberOfSplits);

            Become(Prepared);
        }

        private void Prepared()
        {
            Receive(new Action<StartEmailBatch>(OnStartBatch));
        }

        private void OnStartBatch(StartEmailBatch message)
        {
            for (int i = 0; i < numberOfSplits; i++)
            {
                var feedEmailsRequest = new FeedEmailsRequest(i * NumberOfEmailsInSplit, (i + 1) * NumberOfEmailsInSplit - 1);
                pendingSplitIds.Add(feedEmailsRequest.SplitId);

                message.EmailFeeder.Tell(feedEmailsRequest);

                // TODO Become Completed once all split fed
            }

            Become(AwaitingEmailFeed);
        }

        private void AwaitingEmailFeed()
        {
            Receive(new Action<FeedEmailsReply>(OnFeedEmailsReply));
            ReceiveAny(Unhandled);
        }

        private void OnFeedEmailsReply(FeedEmailsReply message)
        {
            foreach (var email in message.Emails)
                emailSender.Tell(email);

            pendingSplitIds.Remove(message.SplitId);

            if (pendingSplitIds.Count == 0)
                Context.Parent.Tell(new BatchCompleted(batchId));
        }
    }
}