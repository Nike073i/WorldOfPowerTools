using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.API.Test.Infrastructure.Helpers.Models.ValueObject
{
    public static class AddressHelper
    {
        public const string TestCountry = "testCountry";
        public const string TestCity = "testCity";
        public const string TestStreet = "testStreet";
        public const string TestHouse = "testHouse";
        public const int TestFlat = 100;
        public const string TestPostalCode = "testPostalCode";
        public static Address CreateAddress(string county = TestCountry, string city = TestCity, string street = TestStreet,
            string house = TestHouse, int flat = TestFlat, string postalCode = TestPostalCode)
        {
            return new Address(county, city, street, house, flat, postalCode);
        }
    }
}
