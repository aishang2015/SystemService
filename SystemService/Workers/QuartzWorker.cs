using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Spi;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SystemService.QuartzSchedule;

namespace SystemService.Workers
{

    /// <summary>
    /// Quartz服务，启动quartz，加载所有任务
    /// </summary>
    public class QuartzWorker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<JobConfig> _jobConfigs;

        public QuartzWorker(
            ILogger<QuartzWorker> logger,
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory,
            IOptions<List<JobConfig>> jobOptions)
        {
            _logger = logger;
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _jobConfigs = jobOptions.Value.Where(j => j.IsActive && j.JobType != null);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("start initial quartz scheduler.");
            var Scheduler = await _schedulerFactory.GetScheduler(stoppingToken);
            Scheduler.JobFactory = _jobFactory;
            foreach (var jobConfig in _jobConfigs)
            {
                // 创建job
                var job = CreateJob(jobConfig);

                // 创建触发器
                var trigger = CreateTrigger(jobConfig);

                // 添加调度
                await Scheduler.ScheduleJob(job, trigger, stoppingToken);

                _logger.LogInformation($"added {jobConfig.Job} job.");
            }

            // 执行调度器
            await Scheduler.Start(stoppingToken);
            _logger.LogInformation("quartz scheduler already start.");
        }


        /// <summary>
        /// 生成工作实例
        /// </summary>
        private static IJobDetail CreateJob(JobConfig schedule)
        {
            var jobType = schedule.JobType;
            return JobBuilder
                .Create(jobType)
                .SetJobData(new JobDataMap(schedule.DataDic))
                .WithDescription(jobType.Name)
                .Build();
        }

        /// <summary>
        /// 生成触发器实例
        /// </summary>
        private static ITrigger CreateTrigger(JobConfig schedule)
        {
            return TriggerBuilder
                .Create()
                .WithCronSchedule(schedule.Cron)
                .WithDescription(schedule.Cron)
                .Build();
        }
    }
}
