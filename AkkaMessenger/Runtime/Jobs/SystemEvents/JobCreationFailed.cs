using System;

namespace AkkaMessenger.Runtime.Jobs.SystemEvents
{
    class JobCreationFailed : Failure, IJobEvent
    {
        public JobId JobId { get; }

        public JobCreationFailed(JobId jobId, string errorMessage, Exception exception = null) : base(errorMessage, exception)
        {
            JobId = jobId;
        }
    }
}