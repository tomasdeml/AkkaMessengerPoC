namespace AkkaMessenger.Runtime.Recipients.Messages
{
    class CompleteParsing    
    {
        public int NumberOfParsedRecipients { get; private set; }

        public CompleteParsing(int numberOfParsedRecipients)
        {
            NumberOfParsedRecipients = numberOfParsedRecipients;
        }
    }
}