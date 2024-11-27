using System;
using System.Collections.Generic;
using System.Linq;
using TradingEngineServer.Orders;
using TradingEngineServer.Orders.OrderBook;
using TradingEngineSever.Instrument;

namespace TradingEngineServer.Orderbook
{
	public class Orderbook : IRetrievalOrderbook
	{
        private readonly Security _instrument;
        private readonly SortedSet<Limit> _askLimits = new SortedSet<Limit>(AskLimitComparer.Comparer);
        private readonly SortedSet<Limit> _bidLimits = new SortedSet<Limit>(BidLimitComparer.Comparer);
        private readonly Dictionary<long, OrderBookEntry> _orders = new Dictionary<long, OrderBookEntry>();

        public Orderbook(Security instrument)
		{
            _instrument = instrument;
		}

        public int Count => _orders.Count;

        public void AddOrder(Order order)
        {
            var baseLimit = new Limit(order.Price);
            AddOrder(order, baseLimit, order.IsBuySide ? _bidLimits : _askLimits, _orders);
        }

        private void AddOrder(Order order, Limit baseLimit, SortedSet<Limit> limitLevels, Dictionary<long, OrderBookEntry> interalOrderbook)
        {
            if (limitLevels.TryGetValue(baseLimit, out Limit limit))
            {
                OrderBookEntry orderbookEntry = new OrderBookEntry(order, baseLimit);
                if (limit.Head == null)
                {
                    limit.Head = orderbookEntry;
                    limit.Tail = orderbookEntry;
                } else
                {
                    OrderBookEntry tailPointer = limit.Tail;
                    tailPointer.Next = orderbookEntry;
                    orderbookEntry.Previous = tailPointer;
                    limit.Tail = orderbookEntry;
                }
                interalOrderbook.Add(order.OrderId, orderbookEntry);
            }
            else
            {
                limitLevels.Add(baseLimit);
                OrderBookEntry orderbookEntry = new OrderBookEntry(order, baseLimit);
                baseLimit.Head = orderbookEntry;
                baseLimit.Tail = orderbookEntry;
                interalOrderbook.Add(order.OrderId, orderbookEntry);
            }
        }

        public void ChangeOrder(ModifyOrder modifyOrder)
        {
            if (_orders.TryGetValue(modifyOrder.OrderId, out OrderBookEntry orderbookEntry))
            {
                RemoveOrder(modifyOrder.ToCancelOrder());
                AddOrder(modifyOrder.ToNewOrder(), orderbookEntry.ParentLimit, modifyOrder.ModifyIsBuySide ? _bidLimits : _askLimits, _orders);

            }
        }

        public bool ContainsOrder(long orderId)
        {
            return _orders.ContainsKey(orderId);
        }

        public List<OrderBookEntry> GetAskOrders()
        {
            List<OrderBookEntry> orderbookEntries = new List<OrderBookEntry>();
            foreach (var askLimit in _askLimits)
            {
                if (askLimit.IsEmpty)
                {
                    continue;
                } else
                {
                    OrderBookEntry askLimitPointer = askLimit.Head;
                    while (askLimitPointer != null)
                    {
                        orderbookEntries.Add(askLimitPointer);
                        askLimitPointer = askLimitPointer.Next;
                    }
                }

            }

            return orderbookEntries;
        }

        public List<OrderBookEntry> GetBidOrders()
        {
            List<OrderBookEntry> orderbookEntries = new List<OrderBookEntry>();
            foreach (var bidLimit in _bidLimits)
            {
                if (bidLimit.IsEmpty)
                {
                    continue;
                }
                else
                {
                    OrderBookEntry bidLimitPointer = bidLimit.Head;
                    while (bidLimitPointer != null)
                    {
                        orderbookEntries.Add(bidLimitPointer);
                        bidLimitPointer = bidLimitPointer.Next;
                    }
                }

            }

            return orderbookEntries;
        }

        public OrderbookSpread GetSpread()
        {
            long? bestAsk = null;
            long? bestBid = null;

            if (_askLimits.Any() && !_askLimits.Min.IsEmpty)
            {
                bestAsk = _askLimits.Min.Price;
            }

            if (_bidLimits.Any() && !_bidLimits.Max.IsEmpty)
            {
                bestBid = _askLimits.Max.Price;
            }

            return new OrderbookSpread(bestBid, bestAsk);
        }

        public void RemoveOrder(CancelOrder cancelOrder)
        {
            if (_orders.TryGetValue(cancelOrder.OrderId, out OrderBookEntry orderbookEntry))
            {
                RemoveOrder(cancelOrder.OrderId, orderbookEntry, _orders);
            }
        }

        private void RemoveOrder(long orderId, OrderBookEntry orderbookEntry, Dictionary<long, OrderBookEntry> orders)
        {
            if (orderbookEntry.Previous != null && orderbookEntry.Next != null)
            {
                // order is in the middle
                orderbookEntry.Next.Previous = orderbookEntry.Previous;
                orderbookEntry.Previous.Next = orderbookEntry.Next;
            } else if (orderbookEntry.Previous != null)
            {
                // it is the last element
                orderbookEntry.Previous.Next = null;
            } else if (orderbookEntry.Next != null)
            {
                // it is the first element
                orderbookEntry.Next.Previous = null;
            }


            if (orderbookEntry.ParentLimit.Head == orderbookEntry && orderbookEntry.ParentLimit.Tail == orderbookEntry)
            {
                // only one entry in the orderbook
                orderbookEntry.ParentLimit.Head = null;
                orderbookEntry.ParentLimit.Tail = null;
            } else if (orderbookEntry.ParentLimit.Head == orderbookEntry)
            {
                orderbookEntry.ParentLimit.Head = orderbookEntry.Next;
            } else if (orderbookEntry.ParentLimit.Tail == orderbookEntry)
            {
                orderbookEntry.ParentLimit.Tail = orderbookEntry.Previous;
            }


            orders.Remove(orderId);
        }
    }
}

