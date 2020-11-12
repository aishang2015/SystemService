using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SystemService.QuartzSchedule
{
    public class JobConfig
    {
        public string Job { get; set; }

        public string Cron { get; set; }

        public bool IsActive { get; set; }

        public Dictionary<string, string> DataDic { get; set; } = new Dictionary<string, string>();

        public Type JobType
        {
            get => Assembly.Load("SystemService").GetTypes().FirstOrDefault(t => t.Name == Job);
        }

    }
}
