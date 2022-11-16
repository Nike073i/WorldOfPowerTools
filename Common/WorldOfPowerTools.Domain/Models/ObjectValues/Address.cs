using System.ComponentModel.DataAnnotations;

namespace WorldOfPowerTools.Domain.Models.ObjectValues
{
    public class Address
    {
        [Required]
        public string Country { get; protected set; }

        [Required]
        public string City { get; protected set; }

        [Required]
        public string Street { get; protected set; }

        [Required]
        public string House { get; protected set; }

        [Required]
        public int Flat { get; protected set; }

        [Required]
        public string PostalCode { get; protected set; }

#nullable disable
        protected Address() { }

        public Address(string country, string city, string street, string house, int flat, string postalCode)
        {
            if (string.IsNullOrEmpty(country)) throw new ArgumentNullException(nameof(country));
            if (string.IsNullOrEmpty(city)) throw new ArgumentNullException(nameof(city));
            if (string.IsNullOrEmpty(street)) throw new ArgumentNullException(nameof(street));
            if (string.IsNullOrEmpty(house)) throw new ArgumentNullException(nameof(house));
            if (string.IsNullOrEmpty(postalCode)) throw new ArgumentNullException(nameof(postalCode));
            if (flat < 1 || flat > 9999) throw new ArgumentOutOfRangeException(nameof(flat));

            Country = country;
            City = city;
            Street = street;
            House = house;
            Flat = flat;
            PostalCode = postalCode;
        }

        public override bool Equals(object obj)
        {
            return (obj is Address other) && Equals(other);
        }

        public bool Equals(Address other)
        {
            if (other == null) return false;
            return Country == other.Country &&
                City == other.City &&
                Street == other.Street &&
                House == other.House &&
                Flat == other.Flat &&
                PostalCode == other.PostalCode;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Country, City, Street, House, Flat, PostalCode);
        }

        public override string ToString()
        {
            return $"{Country}, г.{City}, ул.{Street}, д.{House}, кв.{Flat}, индекс {PostalCode}";
        }
    }
}
