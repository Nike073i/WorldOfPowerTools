namespace WorldOfPowerTools.Domain.ObjectValues
{
    public class PersonData
    {
        public string FirstName { get; }
        public string SecondName { get; }
        public DateTime Birthday { get; }

        public PersonData(string firstName, string secondName, DateTime birthday)
        {
            throw new System.Exception("Not implemented");
        }
    }
}