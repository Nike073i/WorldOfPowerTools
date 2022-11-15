namespace WorldOfPowerTools.Domain.Exceptions
{
    public class EntityCouldNotBeRemovedException : Exception
    {
        public EntityCouldNotBeRemovedException(string message) : base(message) { }
    }
}
