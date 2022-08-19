using System;

namespace Mosaico.BackgroundJobs.Base
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class BackgroundJobAttribute : Attribute
    {
        public BackgroundJobAttribute(string name, string queue = "default", bool isRecurring = true, string cron = "0 0 * * *")
        {
            Queue = queue;
            Name = name;
            Cron = cron;
            IsRecurring = isRecurring;
        }

        public string Queue { get; set; }
        public string Name { get; set; }
        public string Cron { get; set; }
        public bool IsRecurring { get; set; }
        public bool ExecutedOnStartup { get; set; }
    }
}