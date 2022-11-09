using NUnit.Framework;
using System;
using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.Domain.Test.Models.ObjectValues
{
    public class PersonDataTest
    {
        [Test]
        [TestCaseSource(nameof(IncorrectConstructCases))]
        public void CreatePersonDataIncorrect(string firstName, string secondName, DateTime birthday, Type awaitingException)
        {
            TestDelegate construct = () => new PersonData(firstName, secondName, birthday);
            Assert.Throws(awaitingException, construct);
        }

        [Test]
        public void CreatePersonDataCorrect()
        {
            var expPersonData = new PersonData("Тестовое имя", "Тестовая фамилия", new DateTime(2001, 09, 12));
            var personData = CreatePersonData();
            Assert.AreEqual(expPersonData, personData);
        }

        private PersonData CreatePersonData()
        {
            string firstName = "Тестовое имя";
            string secondName = "Тестовая фамилия";
            var birthday = new DateTime(2001, 09, 12);
            return new PersonData(firstName, secondName, birthday);
        }

        static object[] IncorrectConstructCases =
        {
            new object?[] { "", "Тестовая фамилия", new DateTime(2001, 09, 12), typeof(ArgumentNullException) },
            new object?[] { null, "Тестовая фамилия", new DateTime(2001, 09, 12), typeof(ArgumentNullException) },
            new object?[] { "Тестовое имя", "", new DateTime(2001, 09, 12), typeof(ArgumentNullException) },
            new object?[] { "Тестовое имя", null, new DateTime(2001, 09, 12), typeof(ArgumentNullException) },
            new object?[] { "Тестовое имя", "Тестовая фамилия", PersonData.BirthdayMinDate.AddDays(-1), typeof(ArgumentException) },
            new object?[] { "Тестовое имя", "Тестовая фамилия", PersonData.BirthdayMaxDate.AddDays(1), typeof(ArgumentException) },
        };
    }
}
