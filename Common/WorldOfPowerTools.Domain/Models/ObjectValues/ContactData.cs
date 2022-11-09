namespace WorldOfPowerTools.Domain.Models.ObjectValues
{
    public class ContactData
    {
        public string ContactNumber { get; protected set; }
        public string Email { get; protected set; }

#nullable disable
        protected ContactData() { }

        public ContactData(string contactNumber, string email)
        {
            throw new Exception("Not implemented");
        }
    }
}
