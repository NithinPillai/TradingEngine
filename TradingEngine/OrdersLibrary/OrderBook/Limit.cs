using System;
using System.Collections.Generic;
using TradingEngineServer.Orders.OrderBook;

namespace TradingEngineServer.Orders.OrderBook
{
    // represents given price level in order
    public class Limit
	{
		public long Price { get; private set; }
		public OrderBookEntry Head { get; set; }
		public OrderBookEntry Tail { get; set; }

		public Limit(long price)
		{
			Price = price;
		}



		// at this given price, are there any entries? 
		public bool IsEmpty { get { return Head == null && Tail == null; } }

		public Side Side
		{
			get
			{
				if (IsEmpty)
				{
					return Side.Unknown;
				} else
				{
					return Head.CurrentOrder.IsBuySide ? Side.Bid : Side.Ask;
				}
			}
		}

		public uint GetLevelOrderCount()
		{
			uint orderCount = 0;
			OrderBookEntry curr = Head;
			while (curr != null)
			{
				if (curr.CurrentOrder.CurrentQuantity != 0)
				{
					orderCount++;
				}
				curr = curr.Next;
			}

			return orderCount;
		}

        public uint GetLevelOrderQuantity()
        {
            uint orderQuantity = 0;
            OrderBookEntry curr = Head;
            while (curr != null)
            {
                if (curr.CurrentOrder.CurrentQuantity != 0)
                {
					orderQuantity += curr.CurrentOrder.CurrentQuantity;
                }
                curr = curr.Next;
            }

            return orderQuantity;
        }

		// OrderRecord created to make it not mutable 
		public List<OrderRecord> GetLevelOrderRecords()
		{
			List<OrderRecord> orderRecords = new List<OrderRecord>();
			OrderBookEntry curr = Head;
			uint theoreticalQueuePosition = 0;

			while (curr != null)
			{
				var currentOrder = curr.CurrentOrder;
				if (currentOrder.CurrentQuantity != 0)
				{
					orderRecords.Add(new OrderRecord(currentOrder.OrderId, currentOrder.CurrentQuantity, Price, currentOrder.IsBuySide, currentOrder.Username, currentOrder.SecurityId, theoreticalQueuePosition));
				}

                theoreticalQueuePosition++;
                curr = curr.Next;
            }

			return orderRecords;
		}

    }
}

