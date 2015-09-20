namespace AkkaMessenger.Runtime.Jobs.Messages
{
    class InitializeRecipients
    {
        public string RecipientsDataPath { get; private set; }

        public int RecipientLimit { get; private set; }

        public InitializeRecipients(string recipientsDataPath, int recipientLimit)
        {
            RecipientsDataPath = recipientsDataPath;
            RecipientLimit = recipientLimit;
        }
    }
}