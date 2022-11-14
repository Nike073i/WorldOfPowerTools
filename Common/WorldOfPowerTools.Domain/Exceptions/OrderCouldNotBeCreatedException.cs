namespace WorldOfPowerTools.Domain.Exceptions
{
    public class OrderCouldNotBeCreatedException : Exception
    {
        public OrderCouldNotBeCreatedException(string message) : base(message) { }
    }
}
