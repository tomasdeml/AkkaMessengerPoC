using Akka.Actor;

namespace AkkaMessenger.Runtime.Jobs.SystemEvents
{
    class JobCreated : Success, IJobEvent
    {
        public JobId JobId { get; }
        public IActorRef Job { get; }
 
        public JobCreated(JobId jobId, IActorRef job)
        {
            JobId = jobId;
            Job = job;
        }

        public override string ToString()
        {
            return $"Job {JobId} created";
        }
    }
}