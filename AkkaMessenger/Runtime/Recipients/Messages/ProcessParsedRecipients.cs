using System.Collections.Generic;

namespace AkkaMessenger.Runtime.Recipients.Messages
{
    class ProcessParsedRecipients
    {
        public IDictionary<string, string>[] RecipientValues { get; private set; }

        public ProcessParsedRecipients(IDictionary<string, string>[] recipientValues)
        {
            RecipientValues = recipientValues;
        }
    }
}