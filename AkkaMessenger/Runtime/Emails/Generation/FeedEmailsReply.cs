using System.Collections.Generic;

namespace AkkaMessenger.Runtime.Emails.Generation
{
    class FeedEmailsReply
    { 
        public string SplitId { get; set; }
        public IList<Email> Emails { get; set; }

        public FeedEmailsReply(FeedEmailsRequest request, IList<Email> emails)
        {
            SplitId = request.SplitId;
            Emails = emails;
        }
    }
}