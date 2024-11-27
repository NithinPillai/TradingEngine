using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using TradingEngineServer.Core.Configuration;
using TradingEngineServer.Logging;
using TradingEngineServer.Logging.LoggingConfiguration;

namespace TradingEngineServer.Core
{
	// sealed makes not overridable
	sealed public class TradingEngineServerHostBuilder
	{
		public static IHost BuildTradingEngineServer()
			=> Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
			{
				// start with the configuration

				services.AddOptions();
				services.Configure<TradingEngineServerConfiguration>(context.Configuration.GetSection(nameof(TradingEngineServerConfiguration)));
				services.Configure<LoggerConfiguration>(context.Configuration.GetSection(nameof(LoggerConfiguration)));

				// only meant to be one instance of trading engine server
				services.AddSingleton<ITradingEngineServer, TradingEngineServer>();
				services.AddSingleton<ITextLogger, TextLogger>();

                // adding hosted services has to be of type BackgroundService which TradingEngineServer implements
                services.AddHostedService<TradingEngineServer>();

            }).Build();
		// takes in a context that the service will be build including configuration objects
		// also gives access into services
	}
}

