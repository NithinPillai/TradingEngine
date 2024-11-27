using System;
namespace TradingEngineServer.Core
{
	interface ITradingEngineServer
	{
		Task Run(CancellationToken token);
	}
}

