using System.Collections.Generic;

namespace AkkaMessenger.Runtime.Jobs
{
    class JobRecipient
    {
        public IDictionary<string, string> Values { get; private set; }

        public bool IsValid { get; set; }
 
        public JobRecipient(IDictionary<string, string> values)
        {
            Values = values;
        }
    }
}
