using NUnit.Framework;
using System;
using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.Domain.Test.Models.ObjectValues
{
    public class ContactDataTest
    {
        [Test]
        [TestCaseSource(nameof(IncorrectConstructCases))]
        public void CreateContactDataIncorrect(string contactNumber, string email, Type awaitingException)
        {
            TestDelegate construct = () => new ContactData(contactNumber, email);
            Assert.Throws(awaitingException, construct);
        }

        [Test]
        public void CreateContactDataCorrect()
        {
            var expContactData = new ContactData("+74999999999", "test@mail.ru");
            var contactData = CreateContactData();
            Assert.AreEqual(expContactData, contactData);
        }

        private ContactData CreateContactData()
        {
            string contactNumber = "+74999999999";
            string email = "test@mail.ru";
            return new ContactData(contactNumber, email);
        }

        static object[] IncorrectConstructCases =
        {
            new object?[] { "", "test@mail.ru", typeof(ArgumentNullException) },
            new object?[] { null, "test@mail.ru", typeof(ArgumentNullException) },
            new object?[] { "+74999999999", "",  typeof(ArgumentNullException) },
            new object?[] { "+74999999999", null, typeof(ArgumentNullException) },
            new object[] { "incorrect", "test@mail.ru", typeof(ArgumentException) },
            new object[] { "+74999999999", "incorrect", typeof(ArgumentException) }
        };
    }
}
