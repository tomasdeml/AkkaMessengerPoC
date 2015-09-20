namespace AkkaMessenger.Runtime.Emails.Generation
{
    class FeedEmailsRequest
    {
        public string SplitId { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }

        public FeedEmailsRequest(int fromId, int toId)
        {
            SplitId = $"{fromId}-{toId}";
            FromId = fromId;
            ToId = toId;
        }
    }
}