using System;
namespace TradingEngineServer.Orders.StatusClasses.Reject
{
	public sealed class RejectCreator
	{
		public static RejectStatus GenerateOrderCoreRejection(IOrderCore rejectedOrder, RejectionReason rejectionReason)
		{
			return new RejectStatus(rejectedOrder, rejectionReason);
		}
	}
}

