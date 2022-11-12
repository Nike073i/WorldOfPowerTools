namespace WorldOfPowerTools.Domain.Exceptions
{
    public class UserExistsException : Exception
    {
        public UserExistsException(string message)
        : base(message) { }
    }
}
