namespace WorldOfPowerTools.Domain.Models.ObjectValues
{
    public class PersonData
    {
        public static readonly DateTime BirthdayMinDate = new(1900, 01, 01);
        public static readonly DateTime BirthdayMaxDate = DateTime.Today;

        public string FirstName { get; protected set; }
        public string SecondName { get; protected set; }
        public DateTime Birthday { get; protected set; }

#nullable disable
        protected PersonData() { }

        public PersonData(string firstName, string secondName, DateTime birthday)
        {
            if (string.IsNullOrEmpty(firstName)) throw new ArgumentNullException(nameof(firstName));
            if (string.IsNullOrEmpty(secondName)) throw new ArgumentNullException(nameof(secondName));

            if (birthday < BirthdayMinDate || birthday > BirthdayMaxDate) throw new ArgumentException("Дата рождения указана неверно");

            FirstName = firstName;
            SecondName = secondName;
            Birthday = birthday;
        }

        public override bool Equals(object obj)
        {
            return (obj is PersonData other) && Equals(other);
        }

        public bool Equals(PersonData other)
        {
            if (other == null) return false;
            return FirstName == other.FirstName &&
                SecondName == other.SecondName &&
                Birthday == other.Birthday;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, SecondName, Birthday);
        }

        public override string ToString()
        {
            return $"{FirstName}, {SecondName}, {Birthday.ToShortDateString}";
        }
    }
}