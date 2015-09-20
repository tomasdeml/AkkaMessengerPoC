namespace AkkaMessenger.Runtime.Recipients.Messages
{
    class StartParsing
    {
        public string DataPath { get; private set; }
        public int RecipientLimit { get; private set; }

        public StartParsing(string dataPath, int recipientLimit)
        {
            DataPath = dataPath;
            RecipientLimit = recipientLimit;
        }
    }
}