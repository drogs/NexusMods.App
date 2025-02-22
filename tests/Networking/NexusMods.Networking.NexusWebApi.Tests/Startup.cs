using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NexusMods.Common;
using NexusMods.Common.OSInterop;
using NexusMods.DataModel;
using NexusMods.Networking.HttpDownloader;
using NexusMods.Networking.HttpDownloader.Tests;
using NexusMods.Networking.NexusWebApi.NMA;
using NexusMods.Paths;
using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;

namespace NexusMods.Networking.NexusWebApi.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var mockOsInterop = new Mock<IOSInterop>();

        services
            .AddFileSystem()
            .AddSingleton<HttpClient>()
            .AddLogging(builder => builder.SetMinimumLevel(LogLevel.Debug))
            .AddHttpDownloader()
            .AddSingleton<TemporaryFileManager>()
            .AddSingleton<IProcessFactory, ProcessFactory>()
            .AddSingleton(mockOsInterop.Object)
            .AddSingleton<LocalHttpServer>()
            .AddNexusWebApi()
            .AddNexusWebApiNmaIntegration(true)
            .AddDataModel(new DataModelSettings()
            {
                UseInMemoryDataModel = true
            })
            .Validate();
    }

    public void Configure(ILoggerFactory loggerFactory, ITestOutputHelperAccessor accessor) =>
        loggerFactory.AddProvider(new XunitTestOutputLoggerProvider(accessor, delegate { return true; }));
}

