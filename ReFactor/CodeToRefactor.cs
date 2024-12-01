using System;
using System.Collections.Generic;
using System.Linq;

namespace CodingAssessment.Refactor
{
    public class People
    {
        private static readonly DateTimeOffset Under16 = DateTimeOffset.UtcNow.AddYears(-16);
        private static readonly Random Random = new Random();

        public string Name { get; }
        public DateTimeOffset DOB { get; }

        public People(string name) : this(name, Under16.Date)
        {
        }

        public People(string name, DateTime dob)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));

            if (dob > DateTime.UtcNow)
                throw new ArgumentException("Date of birth cannot be in the future", nameof(dob));

            Name = name;
            DOB = dob;
        }
    }

    public class BirthingUnit
    {
        private static readonly Random Random = new Random();
        private static readonly int DaysInYear = 365;
        private static readonly string[] PossibleNames = { "Bob", "Betty" };
        private readonly List<People> _people = new List<People>();

        public List<People> GetPeople(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be positive", nameof(count));

            for (int i = 0; i < count; i++)
            {
                try
                {
                    var name = PossibleNames[Random.Next(PossibleNames.Length)];
                    var ageInYears = Random.Next(18, 85);
                    var dob = DateTime.UtcNow.AddDays(-ageInYears * DaysInYear);

                    _people.Add(new People(name, dob));
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Failed to create person", ex);
                }
            }

            return _people;
        }

        public IEnumerable<People> GetBobsByAge(bool olderThan30)
        {
            var thirtyYearsAgo = DateTime.UtcNow.AddYears(-30);
            return _people.Where(x => x.Name == "Bob" &&
                (olderThan30 ? x.DOB <= thirtyYearsAgo : x.DOB > thirtyYearsAgo));
        }

        public string GetMarriedName(People person, string lastName)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be null or empty", nameof(lastName));

            if (lastName.Contains("test", StringComparison.OrdinalIgnoreCase))
                return person.Name;

            var fullName = $"{person.Name} {lastName}";
            return fullName.Length > 255 ? fullName[..255] : fullName;
        }
    }
}