using System;
using System.Collections.Generic;

namespace AkkaMessenger
{
    class FeedEmailsReply
    { 
        public Guid SplitId { get; set; }
        public IList<Email> Emails { get; set; }

        public FeedEmailsReply(FeedEmailsRequest request, IList<Email> emails)
        {
            SplitId = request.SplitId;
            Emails = emails;
        }
    }
}