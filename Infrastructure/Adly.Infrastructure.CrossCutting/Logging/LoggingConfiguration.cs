using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;

namespace Adly.Infrastructure.CrossCutting.Logging;

public class LoggingConfiguration
{
    public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger => (context, configuration) =>
    {
        var applicationName = context.HostingEnvironment.ApplicationName;
        configuration
            .Enrich.WithSpan()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .Enrich.WithExceptionDetails().
            Enrich.WithProperty("ApplicationName", applicationName);


        if (context.HostingEnvironment.IsDevelopment())
        {
            configuration.WriteTo.Console().MinimumLevel.Information();
            return;
        }


        if (context.HostingEnvironment.IsProduction())
        {
            configuration.WriteTo.Console().MinimumLevel.Error();
            return;
        }





    };
}