using NUnit.Framework;
using System;
using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.Domain.Test.Models.ObjectValues
{
    public class PersonDataTest
    {
        private static string testFirstName = "Тестовое имя";
        private static string testSecondName = "Тестовая фамилия";
        private static DateTime testBirthday = new DateTime(2001, 09, 12);

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
            var expPersonData = new PersonData(testFirstName, testSecondName, testBirthday);
            var personData = CreatePersonData();
            Assert.AreEqual(expPersonData, personData);
        }

        private PersonData CreatePersonData()
        {
            string firstName = testFirstName;
            string secondName = testSecondName;
            var birthday = testBirthday;
            return new PersonData(firstName, secondName, birthday);
        }

        static object[] IncorrectConstructCases =
        {
            new object?[] { "", testSecondName, testBirthday, typeof(ArgumentNullException) },
            new object?[] { null, testSecondName, testBirthday, typeof(ArgumentNullException) },
            new object?[] { testFirstName, "", testBirthday, typeof(ArgumentNullException) },
            new object?[] { testFirstName, null, testBirthday, typeof(ArgumentNullException) },
            new object?[] { testFirstName, testSecondName, PersonData.BirthdayMinDate.AddDays(-1), typeof(ArgumentException) },
            new object?[] { testFirstName, testSecondName, PersonData.BirthdayMaxDate.AddDays(1), typeof(ArgumentException) },
        };
    }
}
