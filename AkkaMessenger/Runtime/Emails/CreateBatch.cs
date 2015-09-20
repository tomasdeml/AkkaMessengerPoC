using System;

namespace AkkaMessenger.Runtime.Emails
{
    class CreateBatch : IBatchBound
    {
        public Guid BatchId { get; set; }
        public int NumberOfEmails { get; set; }

        public CreateBatch(Guid batchId, int numberOfEmails)
        {
            BatchId = batchId;
            NumberOfEmails = numberOfEmails;
        }
    }
}