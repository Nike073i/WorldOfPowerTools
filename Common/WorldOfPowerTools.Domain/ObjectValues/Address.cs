namespace WorldOfPowerTools.Domain.ObjectValues
{
    public class Address
    {
        public string Country { get; protected set; }
        public string City { get; protected set; }
        public string Street { get; protected set; }
        public string House { get; protected set; }
        public int Flat { get; protected set; }
        public string PostalCode { get; protected set; }

#nullable disable
        protected Address() { }

        public Address(string country, string city, string street, string house, int flat, string postalCode)
        {
            throw new System.Exception("Not implemented");
        }
    }
}
