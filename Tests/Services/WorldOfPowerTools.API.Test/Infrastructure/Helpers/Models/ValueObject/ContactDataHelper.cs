using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.API.Test.Infrastructure.Helpers.Models.ValueObject
{
    public static class ContactDataHelper
    {
        public const string TestContactNumber = "+79475912545";
        public const string TestEmail = "kit013i@mipl.ry";
        public static ContactData CreateContactData(string contactNumber = TestContactNumber, string email = TestEmail)
        {
            return new ContactData(contactNumber, email);
        }
    }
}
