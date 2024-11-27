using System;
namespace TradingEngineServer.Orders.StatusClasses.Reject
{
	public enum RejectionReason
	{
		Unknown,
		OrderNotFound,
		InstrumentNotFound,
		AttemptingToModifyWrongSide
	}
}

