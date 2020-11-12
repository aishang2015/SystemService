using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Threading.Tasks;

namespace SystemService.QuartzSchedule
{
    /// <summary>
    /// 通过此job来运行其他job
    /// </summary>
    public class QuartzJobRunner : IJob
    {
        private readonly IServiceProvider _serviceProvider;

        public QuartzJobRunner(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 执行job
        /// </summary>
        public async Task Execute(IJobExecutionContext context)
        {
            // 手动创建scope
            using (var scope = _serviceProvider.CreateScope())
            {
                var jobType = context.JobDetail.JobType;
                var job = scope.ServiceProvider.GetRequiredService(jobType) as IJob;
                await job.Execute(context);
            }
        }
    }
}
