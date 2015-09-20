namespace AkkaMessenger.Runtime.Jobs.Messages
{
    class CreateJob
    {
        public JobId JobId { get; private set; }

        public CreateJob(JobId jobId)
        {
            JobId = jobId;
        }
    }
}