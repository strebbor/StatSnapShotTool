using Autofac;
using Microsoft.Extensions.Configuration;
using StatSnapShotter.Entities;
using StatSnapShotter.Interfaces;
using Serilog;
using System;
using System.IO;

namespace StatSnapShotter
{
    public static class ContainerConfig
    {
        public static IConfiguration ConfigureAppSetup(string environment)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .Build();
            return configuration;
        }      
        public static IContainer Configure(string environment)
        {
            var configuration = ConfigureAppSetup(environment);

            var builder = new ContainerBuilder();
            builder.RegisterType<StatProcessor>().As<IApplication>();

            SetupLogger(configuration);
            SetupStatSettings(configuration, builder);
            SetupDataSource(configuration, builder);
            SetupDataStorage(configuration, builder);

            return builder.Build();
        }

        private static void SetupLogger(IConfiguration configuration)
        {
            var settings = new DataProviderSettings();
            configuration.GetSection("LoggingProvider").Bind(settings);
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "SeqSnapShotTool")
            .WriteTo.Seq(settings.Location.ToString(), Serilog.Events.LogEventLevel.Verbose)
            .CreateLogger();
        }
        private static void SetupStatSettings(IConfiguration configuration, ContainerBuilder builder)
        {
            var settings = new DataProviderSettings();
            configuration.GetSection("ConfigurationProvider").Bind(settings);
            var configurationProvider = CreateAssemblyInstance(settings.AssemblyName.ToString(), settings);

            builder.RegisterType<StatConfiguration>().As<IStatConfiguration>()
                .WithParameter("configurationProvider", configurationProvider)
                .WithParameter("intervalHour", configuration.GetSection("IntervalHour").Value);
        }
        private static void SetupDataStorage(IConfiguration configuration, ContainerBuilder builder)
        {
            var settings = new DataProviderSettings();
            configuration.GetSection("DataStorageProvider").Bind(settings);

            var dataStorageProvider = CreateAssemblyInstance(settings.AssemblyName.ToString(), settings);
            builder.RegisterInstance(dataStorageProvider).Keyed<IDataManipulator>("dataStorage");
        }
        private static void SetupDataSource(IConfiguration configuration, ContainerBuilder builder)
        {
            var settings = new DataProviderSettings();
            configuration.GetSection("DataSourceProvider").Bind(settings);

            var dataSourceProvider = CreateAssemblyInstance(settings.AssemblyName.ToString(), settings);
            builder.RegisterInstance(dataSourceProvider).Keyed<IDataManipulator>("dataSource");
        }
        private static object CreateAssemblyInstance(string assemblyName, params object[] constructorParameters)
        {
            Type instanceType = Type.GetType(assemblyName);
            if (constructorParameters != null)
            {
                return Activator.CreateInstance(instanceType, constructorParameters);
            }
            else
            {
                return Activator.CreateInstance(instanceType);
            }
        }
    }
}
