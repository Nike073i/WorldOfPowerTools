namespace WorldOfPowerTools.Domain.Exceptions
{
    public class OrderChangeStatusException : Exception
    {
        public OrderChangeStatusException(string message) : base(message) { }
    }
}
