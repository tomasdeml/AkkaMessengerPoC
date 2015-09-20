using System;
using Akka.Actor;
using Akka.Event;
using AkkaMessenger.Runtime.Jobs;
using AkkaMessenger.Runtime.Jobs.Messages;
using AkkaMessenger.Runtime.Jobs.SystemEvents;
using Failure = AkkaMessenger.Runtime.Failure;

namespace AkkaMessenger
{
    class InteractionDriverActor : ReceiveActor
    {
        const string RecipientsDataPath = @"D:\Temp\Crimes_-_2001_to_present.csv";

        IActorRef jobRegistry;
        IActorRef job;
        JobId jobId;

        public InteractionDriverActor()
        {
            Become(AwaitingTrigger);
        }

        protected override void PreStart()
        {
            base.PreStart();
            jobRegistry = Context.ActorOf(Props.Create<JobRegistryActor>(), "job-registry");
            Context.System.EventStream.Subscribe<IJobEvent>(Self);
        }

        void AwaitingTrigger()
        {
            ReceiveAny(_ =>
            { 
                jobId = JobId.NewId();
                jobRegistry.Tell(new CreateJob(jobId));
                Become(AwaitingJobCreation);
            });
        }
 
        void AwaitingJobCreation()
        {
            Receive<JobCreated>(ev =>
            { 
                job = ev.Job;
                job.Tell(new InitializeRecipients(RecipientsDataPath, 5000)); 
                Become(AwaitingJobRecipientsInitialization);
            });
            ReceiveFailureByThrowing();
            ReceiveAny(Unhandled);
        }

        void AwaitingJobRecipientsInitialization()
        {
            Receive<RecipientsInitialized>(ev =>
            {
                Context.System.Shutdown();
            });
            ReceiveFailureByThrowing();
            ReceiveAny(Unhandled);
        }

        void ReceiveFailureByThrowing()
        {
            Receive(new Action<Failure>(ev => { throw new ApplicationException(ev.ToString()); }));
        }
    } 
}