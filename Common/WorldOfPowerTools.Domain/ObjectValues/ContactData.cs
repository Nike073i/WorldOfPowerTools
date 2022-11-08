namespace WorldOfPowerTools.Domain.ObjectValues
{
    public class ContactData
    {
        public string ContactNumber { get; }
        public string Email { get; }

#nullable disable
        protected ContactData() { }

        public ContactData(string contactNumber, string email)
        {
            throw new System.Exception("Not implemented");
        }
    }
}
