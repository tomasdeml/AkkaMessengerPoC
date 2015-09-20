namespace AkkaMessenger.Runtime.Jobs.Messages
{
    class GetJob
    {
        public JobId JobId { get; private set; }

        public GetJob(JobId jobId)
        {
            JobId = jobId;
        }
    }
}