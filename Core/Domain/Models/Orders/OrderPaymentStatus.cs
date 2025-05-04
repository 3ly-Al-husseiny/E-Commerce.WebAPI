namespace Domain.Models.Orders
{
    public enum OrderPaymentStatus
    {
        Pending = 0, // Waiting
        PaymentReceived = 1, // Payment Received
        PaymentFailed = 2, // Payment Failed
    }
}