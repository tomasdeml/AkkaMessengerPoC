using System;
using Akka.Actor;
using Akka.Routing;

namespace AkkaMessenger
{
    class MessengerActor : ReceiveActor
    {
        private IActorRef emailSender;

        public MessengerActor()
        {
            Receive(new Action<CreateEmailBatch>(OnCreateEmailBatch));
            Receive(new Action<StartEmailBatch>(OnStartEmailBatch));
        }

        private void OnStartEmailBatch(StartEmailBatch obj)
        {
            
        }

        protected override void PreStart()
        {
            base.PreStart();
            emailSender = Context.ActorOf(Props.Create<EmailSenderActor>().WithRouter(new RoundRobinPool(4)));
        }

        private void OnCreateEmailBatch(CreateEmailBatch message)
        {
            var batchActor = Context.ActorOf(Props.Create<EmailBatchActor>(emailSender), $"batch-{message.BatchId}");
            batchActor.Tell(new PrepareBatch(message.BatchId, message.NumberOfEmails)); 
        }
    }
}
