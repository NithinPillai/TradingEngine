using System; // uses system
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TradingEngineServer.Core;


using var engine = TradingEngineServerHostBuilder.BuildTradingEngineServer(); // builds the engine using the host 
TradingEngineServerServiceProvider.ServiceProvider = engine.Services; // engine returns a IHost and the services from previous line build is set to TESSP for global acces

{
    using var scope = TradingEngineServerServiceProvider.ServiceProvider.CreateScope();

    await engine.RunAsync().ConfigureAwait(false);
}