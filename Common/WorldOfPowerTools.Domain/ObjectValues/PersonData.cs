namespace WorldOfPowerTools.Domain.ObjectValues
{
    public class PersonData
    {
        public string FirstName { get; protected set; }
        public string SecondName { get; protected set; }
        public DateTime Birthday { get; protected set; }

#nullable disable
        protected PersonData() { }

        public PersonData(string firstName, string secondName, DateTime birthday)
        {
            throw new System.Exception("Not implemented");
        }
    }
}