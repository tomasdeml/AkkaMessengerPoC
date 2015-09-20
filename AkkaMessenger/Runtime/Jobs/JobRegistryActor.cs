using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Event;
using AkkaMessenger.Runtime.Jobs.Messages;
using AkkaMessenger.Runtime.Jobs.SystemEvents;

namespace AkkaMessenger.Runtime.Jobs
{
    class JobRegistryActor : ReceiveActor
    {
        readonly IDictionary<JobId, IActorRef> activeJobs;
        ILoggingAdapter logger;

        public JobRegistryActor()
        {
            activeJobs = new Dictionary<JobId, IActorRef>(); 
            Receive(new Action<CreateJob>(OnCreateJob));
            Receive(new Action<GetJob>(OnGetJob));
        }
 
        protected override void PreStart()
        {
            base.PreStart();
            logger = Context.GetLogger();
        }
 
        void OnCreateJob(CreateJob message)
        {
            logger.Debug($"Creating job {message.JobId}");

            if (JobExists(message.JobId))
            {
                Context.PublishEvent(new JobCreationFailed(message.JobId, $"Job with ID {message.JobId} already exists."));
                return;
            }

            var job = CreateJob(message.JobId);
            Context.PublishEvent(new JobCreated(message.JobId, job));
        }

        void OnGetJob(GetJob message)
        {
            IActorRef job;

            Sender.Tell(activeJobs.TryGetValue(message.JobId, out job)
                ? new GetJobReply(job)
                : new GetJobReply(ActorRefs.Nobody));
        }

        IActorRef CreateJob(JobId jobId)
        {
            IActorRef job;
            if (activeJobs.TryGetValue(jobId, out job))
                return job;

            job = Context.ActorOf(Props.Create<JobActor>(jobId), jobId.ToName()); 
            activeJobs[jobId] = job;

            return job;
        }

        bool JobExists(JobId jobId)
        {
            return activeJobs.ContainsKey(jobId);
        } 
    }
}
