using System;
namespace TradingEngineServer.Orders
{
	public class ModifyOrder : IOrderCore
	{
		private readonly IOrderCore _orderCore;

		public ModifyOrder(IOrderCore orderCore, long modifyPrice, uint modifyQuantity, bool modifyIsBuySide)
		{
			_orderCore = orderCore;

			ModifyPrice = modifyPrice;
			ModifyQuantity = modifyQuantity;
			ModifyIsBuySide = modifyIsBuySide;
        }

		public long ModifyPrice { get; private set; }
		public uint ModifyQuantity { get; private set; }
		public bool ModifyIsBuySide { get; private set; }

		public long OrderId => _orderCore.OrderId;
		public string Username => _orderCore.Username;
        public int SecurityId => _orderCore.SecurityId;

		public CancelOrder ToCancelOrder()
		{
			return new CancelOrder(this);
		}

		public Order ToNewOrder()
		{
			return new Order(this);
		}
    }
}

