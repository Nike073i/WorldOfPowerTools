using NUnit.Framework;
using System;
using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.Domain.Test.Models.ObjectValues
{
    public class AddressTest
    {
        [Test]
        [TestCaseSource(nameof(IncorrectConstructCases))]
        public void CreateAddressIncorrect(string country, string city, string street, string house, int flat, string postalCode, Type awaitingException)
        {
            TestDelegate construct = () => new Address(country, city, street, house, flat, postalCode);
            Assert.Throws(awaitingException, construct);
        }

        [Test]
        public void CreateAddressCorrect()
        {
            var expAddress = new Address("Тестовая страна", "Тестовый город", "Тестовая улица", "30б", 100, "443444");
            var address = CreateAddress();
            Assert.AreEqual(expAddress, address);
        }

        public Address CreateAddress()
        {
            string country = "Тестовая страна";
            string city = "Тестовый город";
            string street = "Тестовая улица";
            string house = "30б";
            int flat = 100;
            string postalCode = "443444";
            return new Address(country, city, street, house, flat, postalCode);
        }

        static object[] IncorrectConstructCases =
        {
            new object?[] { "", "Тестовый город", "Тестовая улица", "30б", 100, "443444", typeof(ArgumentNullException) },
            new object?[] { null, "Тестовый город", "Тестовая улица", "30б", 100, "443444", typeof(ArgumentNullException) },
            new object?[] { "Тестовая страна", "", "Тестовая улица", "30б", 100, "443444", typeof(ArgumentNullException) },
            new object?[] { "Тестовая страна", null, "Тестовая улица", "30б", 100, "443444", typeof(ArgumentNullException) },
            new object?[] { "Тестовая страна", "Тестовый город", "", "30б", 100, "443444", typeof(ArgumentNullException) },
            new object?[] { "Тестовая страна", "Тестовый город", null, "30б", 100, "443444", typeof(ArgumentNullException) },
            new object?[] { "Тестовая страна", "Тестовый город", "Тестовая улица", "", 100, "443444", typeof(ArgumentNullException) },
            new object?[] { "Тестовая страна", "Тестовый город", "Тестовая улица", null, 100, "443444", typeof(ArgumentNullException) },
            new object?[] { "Тестовая страна", "Тестовый город", "Тестовая улица", "30б", 0, "443444", typeof(ArgumentOutOfRangeException) },
            new object?[] { "Тестовая страна", "Тестовый город", "Тестовая улица", "30б", 10000, "443444", typeof(ArgumentOutOfRangeException) },
            new object?[] { "Тестовая страна", "Тестовый город", "Тестовая улица", "30б", 100, "", typeof(ArgumentNullException) },
            new object?[] { "Тестовая страна", "Тестовый город", "Тестовая улица", "30б", 100, null, typeof(ArgumentNullException) },
    };
    }
}
