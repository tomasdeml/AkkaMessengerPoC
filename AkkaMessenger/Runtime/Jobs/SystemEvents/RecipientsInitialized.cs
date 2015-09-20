namespace AkkaMessenger.Runtime.Jobs.SystemEvents
{
    class RecipientsInitialized : Success, IJobEvent
    {
        public JobId JobId { get; }

        public RecipientsInitialized(JobId jobId) 
        {
            JobId = jobId;
        }

        public override string ToString()
        {
            return $"Recipients initialized for {JobId}";
        }
    }
}