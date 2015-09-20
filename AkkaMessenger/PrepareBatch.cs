using System;

namespace AkkaMessenger
{
    class PrepareBatch
    {
        public Guid BatchId { get; set; }
        public int NumberOfEmails { get; set; }

        public PrepareBatch(Guid batchId, int numberOfEmails)
        {
            BatchId = batchId;
            NumberOfEmails = numberOfEmails;
        }
    }
}