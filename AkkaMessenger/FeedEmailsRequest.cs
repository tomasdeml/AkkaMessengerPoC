using System;

namespace AkkaMessenger
{
    class FeedEmailsRequest
    {
        public Guid SplitId { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }

        public FeedEmailsRequest(int fromId, int toId)
        {
            SplitId = Guid.NewGuid();
            FromId = fromId;
            ToId = toId;
        }
    }
}