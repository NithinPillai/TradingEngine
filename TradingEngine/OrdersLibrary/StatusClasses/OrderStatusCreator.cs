using System;
namespace TradingEngineServer.Orders.StatusClasses
{
	public sealed class OrderStatusCreator
	{
		public static CancelOrderStatus GenerateCancelOrderStatus(CancelOrder cancelOrder)
		{
			return new CancelOrderStatus();
		}

        public static NewOrderStatus GenerateNewOrderStatus(Order order)
        {
            return new NewOrderStatus();
        }

        public static ModifyOrderStatus GenerateModifyOrdersStatus(ModifyOrder comodifyOrder)
        {
            return new ModifyOrderStatus();
        }
    }
}

