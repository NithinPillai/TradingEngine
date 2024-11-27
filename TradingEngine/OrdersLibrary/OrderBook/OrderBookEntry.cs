using System;
namespace TradingEngineServer.Orders.OrderBook
{
	public class OrderBookEntry
	{
		public OrderBookEntry(Order currentOrder, Limit parentLimit)
		{
			CreationTime = DateTime.UtcNow;
			ParentLimit = parentLimit;
			CurrentOrder = currentOrder;
		}

		public DateTime CreationTime { get; private set; }
		public Order CurrentOrder { get; private set; }
		public Limit ParentLimit { get; private set; }
		public OrderBookEntry Next { get; set; }
		public OrderBookEntry Previous { get; set; }

    }
}

