using System;
namespace TradingEngineServer.Orders.StatusClasses.Reject
{
    public class RejectStatus : IOrderCore
    {
        private readonly IOrderCore _orderCore;

        public RejectStatus(IOrderCore rejectedOrder, RejectionReason rejectionReason)
        {
            _orderCore = rejectedOrder;
            RejectionReason = rejectionReason;
        }

        public long OrderId => _orderCore.OrderId;
        public string Username => _orderCore.Username;
        public int SecurityId => _orderCore.SecurityId;

        public RejectionReason RejectionReason { get; private set; }
    }
}

