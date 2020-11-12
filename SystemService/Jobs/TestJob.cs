using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SystemService.Jobs
{
    public class TestJob : IJob
    {
        private readonly ILogger<TestJob> _logger;

        public TestJob(ILogger<TestJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            context.MergedJobDataMap.Keys.ToList().ForEach(k =>
                _logger.LogInformation($"one param key is {k},value is {context.MergedJobDataMap.GetString(k)}"));
            _logger.LogInformation("Job running at: {time}", DateTimeOffset.Now);
            return Task.CompletedTask;
        }
    }
}
