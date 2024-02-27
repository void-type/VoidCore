using BenchmarkDotNet.Attributes;
using Bogus;
using System.Diagnostics.CodeAnalysis;
using VoidCore.Model.Functional;

namespace VoidCore.Benchmark;

[MemoryDiagnoser]
public class LookupBenchmarks
{
    [Params(30000)]
    public int NumberOfPeople { get; set; }
    public List<Person> People { get; set; } = [];
    public List<string> IdsToFind { get; set; } = [];

    [GlobalSetup]
    public void Setup()
    {
        var faker = new PersonFaker();

        var people = faker
            .Generate(NumberOfPeople);

        IdsToFind = people
            .Take(NumberOfPeople)
            .Select(x => x.Id)
            .ToList();

        People = people
            .OrderBy(x => Guid.NewGuid())
            .ToList();
    }

    [Benchmark]
    public void FirstOrDefault()
    {
        foreach (var id in IdsToFind)
        {
            var s = People.FirstOrDefault(x => x.Id == id);
            var f = s?.FirstName;
        }
    }

    [Benchmark]
    public void Dictionary()
    {
        var dict = People.ToDictionary(x => x.Id);

        foreach (var id in IdsToFind)
        {
            var a = dict.TryGetValue(id, out var s);
            var f = s?.FirstName;
        }
    }

    [Benchmark]
    public void HashSet()
    {
        var hashSet = People.ToHashSet(new PersonComparer());

        foreach (var id in IdsToFind)
        {
            var a = hashSet.TryGetValue(new Person { Id = id }, out var s);
            var f = s?.FirstName;
        }
    }

    public class Person
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
    }

    public class PersonFaker : Faker<Person>
    {
        public PersonFaker()
        {
            RuleFor(e => e.FirstName, f => f.Name.FirstName());
            RuleFor(e => e.LastName, f => f.Name.LastName());
        }
    }

    public class PersonComparer : IEqualityComparer<Person>
    {
        public bool Equals(Person? x, Person? y)
        {
            return x?.Id == y?.Id;
        }

        public int GetHashCode([DisallowNull] Person obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
