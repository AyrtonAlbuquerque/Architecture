namespace Architecture.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class JobScheduleAttribute : Attribute
    {
        public string Cron { get; }

        public JobScheduleAttribute(string cron)
        {
            Cron = cron;
        }
    }
}