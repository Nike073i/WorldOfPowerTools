using System.Text.RegularExpressions;

namespace WorldOfPowerTools.Domain.Models.ObjectValues
{
    public class ContactData
    {
        public static readonly string ContactNumberPattern = @"^\+7\d{10}$";
        public static readonly string EmailPattern = @"^[a-zA-Z0-9]+[@][a-z]{4,}[.][a-z]{2,}";
        public string ContactNumber { get; protected set; }
        public string Email { get; protected set; }

#nullable disable
        protected ContactData() { }

        public ContactData(string contactNumber, string email)
        {
            if (string.IsNullOrEmpty(contactNumber)) throw new ArgumentNullException(nameof(contactNumber));
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));

            if (!Regex.IsMatch(contactNumber, ContactNumberPattern)) throw new ArgumentException("Н.телефона указан неверно");
            if (!Regex.IsMatch(email, EmailPattern)) throw new ArgumentException("E-mail указан неверно");

            ContactNumber = contactNumber;
            Email = email;
        }

        public override bool Equals(object obj)
        {
            return (obj is ContactData other) && Equals(other);
        }

        public bool Equals(ContactData other)
        {
            if (other == null) return false;
            return ContactNumber == other.ContactNumber &&
                Email == other.Email;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ContactNumber, Email);
        }

        public override string ToString()
        {
            return $"н.т. - {ContactNumber}, e-mail - {Email}";
        }
    }
}
