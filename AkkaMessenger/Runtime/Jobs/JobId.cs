using System;

namespace AkkaMessenger.Runtime.Jobs
{
    struct JobId
    {
        const string JobNamePrefix = "job-";
        readonly Guid value;

        JobId(Guid value)
        {
            this.value = value;
        }

        public static JobId NewId()
        {
            return new JobId(Guid.NewGuid());
        }
 
        public string ToName()
        { 
            return JobNamePrefix + value;
        }

        static Guid ParseFrom(string name)
        {
            return Guid.Parse(name.Replace(JobNamePrefix, string.Empty));
        }

        public override string ToString()
        {
            return ToName();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            var other = (JobId) obj;
            return value.Equals(other.value);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static bool operator ==(JobId left, JobId right) 
        {
            return left.Equals(right);
        }

        public static bool operator !=(JobId left, JobId right)
        {
            return !left.Equals(right);
        }
    }
}