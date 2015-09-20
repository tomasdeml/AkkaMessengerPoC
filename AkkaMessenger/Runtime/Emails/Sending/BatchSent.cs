using System;

namespace AkkaMessenger.Runtime.Emails.Sending
{
    class BatchSent
    {
        public Guid BatchId { get; set; }

        public BatchSent(Guid batchId)
        {
            BatchId = batchId;
        }
    }
}