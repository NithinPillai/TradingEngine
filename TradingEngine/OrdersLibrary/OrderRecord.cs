using System;
namespace TradingEngineServer.Orders
{
	public record OrderRecord(long OrderId, uint Quantity, long Price, bool IsBuySide,
		string Username, int SecurityInt, uint TheoreticalQueuePostion);
	// readonly representation of an order
}

namespace System.Runtime.CompilerServices
{
	internal static class IsExternalInit { };
}
