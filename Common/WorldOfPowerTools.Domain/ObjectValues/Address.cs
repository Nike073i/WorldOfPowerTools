namespace WorldOfPowerTools.Domain.ObjectValues
{
    public class Address
    {
        public string Country { get; }
        public string City { get; }
        public string Street { get; }
        public string House { get; }
        public int Flat { get; }
        public string PostalCode { get; }

#nullable disable
        protected Address() { }

        public Address(string country, string city, string street, string house, int flat, string postalCode)
        {
            throw new System.Exception("Not implemented");
        }
    }
}
