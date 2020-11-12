# SystemService

根据WorkerService实现的系统服务

### 依赖

Microsoft.Extensions.Hosting.WindowsServices：windows系统服务

Microsoft.Extensions.Hosting.Systemd：linux服务

NLog.Extensions.Logging：日志

Quartz：计划任务

### 开发

创建自己的job，参照TestJob.cs，这里job可以使用scoped生命周期的对象。appsettings.json中对job进行配置。Program.cs中对容器进行配置。

### 使用：

1.发布，拷贝文件到指定目录。

2.创建，开启，停止，删除服务

```
sc.exe create serviceName binPath=D:\somedir\SystemService.exe   
sc.exe start serviceName
sc.exe stop serviceName
sc.exe delete serviceName
```

