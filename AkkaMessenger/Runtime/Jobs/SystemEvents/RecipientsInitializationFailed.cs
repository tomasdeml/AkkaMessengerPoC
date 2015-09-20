using System;

namespace AkkaMessenger.Runtime.Jobs.SystemEvents
{
    class RecipientsInitializationFailed : Failure, IJobEvent
    {
        public JobId JobId { get; }
        
        public RecipientsInitializationFailed(JobId jobId, string errorMessage, Exception exception = null) : base(errorMessage, exception)
        {
            JobId = jobId;
        }
    }
}