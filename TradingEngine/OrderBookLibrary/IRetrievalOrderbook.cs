using System;
using System.Collections.Generic;
using TradingEngineServer.Orders.OrderBook;

namespace TradingEngineServer.Orderbook
{
	public interface IRetrievalOrderbook : IOrderEntryOrderbook
	{
		// allows me to change the state of the orderbook outside of the orderbook itself

		List<OrderBookEntry> GetAskOrders();
		List<OrderBookEntry> GetBidOrders();
    }
}

