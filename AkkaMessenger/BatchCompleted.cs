using System;

namespace AkkaMessenger
{
    class BatchCompleted
    {
        public Guid BatchId { get; set; }

        public BatchCompleted(Guid batchId)
        {
            BatchId = batchId;
        }
    }
}