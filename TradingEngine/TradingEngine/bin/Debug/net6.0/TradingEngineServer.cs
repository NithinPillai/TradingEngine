using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using TradingEngineServer.Core.Configuration;
using TradingEngineServer.Logging;

namespace TradingEngineServer.Core
{
	// BackgroundService is Microsoft's Hosting NuGet package
	sealed public class TradingEngineServer : BackgroundService, ITradingEngineServer
	{
		private readonly ITextLogger _logger;
		//private readonly ILogger<TradingEngineServer> _logger;

        private readonly TradingEngineServerConfiguration _tradingEngineServerConfig;
        // readonly means it can't be changed after set
        // different from const because const is defined at compile time



        // Ioption is an optional parameter
        public TradingEngineServer(ITextLogger logger, IOptions<TradingEngineServerConfiguration> config)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_tradingEngineServerConfig = config.Value ?? throw new ArgumentNullException(nameof(config));

        }



        public Task Run(CancellationToken token) => ExecuteAsync(token);



        // services have loops, and even if you don't need it, you still need to include it 
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information(nameof(TradingEngineServer), "Starting Trading Engine");
            while (!stoppingToken.IsCancellationRequested)
			{

			}
            _logger.Information(nameof(TradingEngineServer), "Stopping Trading Engine");



            return Task.CompletedTask;
        }
    }
}

