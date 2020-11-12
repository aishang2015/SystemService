using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System.Collections.Generic;

namespace SystemService.QuartzSchedule
{
    public static class QuartzExtension
    {
        public static IServiceCollection AddQuartz(this IServiceCollection services, IConfiguration configuration)
        {
            //添加Quartz服务
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            // 添加jobRunner,将job转为以scope形式运行
            services.AddSingleton<QuartzJobRunner>();

            // 获取job配置，并批量注入
            var jobConfigs = configuration.GetSection("Jobs").Get<List<JobConfig>>();
            services.Configure<List<JobConfig>>(configuration.GetSection("Jobs"));
            jobConfigs.ForEach(jobConfig =>
            {
                if (jobConfig.JobType != null)
                {
                    services.AddScoped(jobConfig.JobType);
                }
            });

            return services;
        }
    }
}
