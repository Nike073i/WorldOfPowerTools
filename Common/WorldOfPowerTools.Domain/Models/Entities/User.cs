using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Extensions;
using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.Domain.Models.Entities
{
    public class User : Entity
    {
        public static readonly int MinLoginLength = 5;
        public static readonly int MaxLoginLength = 50;
        public string Login { get; protected set; }
        public string PasswordHash { get; protected set; }
        public PersonData? PersonData { get; set; }
        public ContactData? ContactData { get; set; }
        public Address? Address { get; set; }
        public Actions Rights { get; protected set; }

#nullable disable
        protected User() { }

        public User(string login, string passwordHash, Actions rights)
        {
            if (string.IsNullOrEmpty(login)) throw new ArgumentNullException(nameof(login));
            if (login.Length < MinLoginLength || login.Length > MaxLoginLength) throw new ArgumentOutOfRangeException(nameof(login));
            if (string.IsNullOrEmpty(passwordHash)) throw new ArgumentNullException(nameof(passwordHash));

            Login = login;
            PasswordHash = passwordHash;
            Rights = rights;
        }

        public User AllowAction(Actions action)
        {
            Rights = Rights.Set(action);
            return this;
        }

        public User ProhibitAction(Actions action)
        {
            Rights = Rights.Clear(action);
            return this;
        }
    }
}
