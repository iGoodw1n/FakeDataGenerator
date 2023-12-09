using Bogus;
using FakeUserDataGenerator.Models;
using System.Globalization;

namespace FakeUserDataGenerator.Services;

public static class FakeDataService
{
    public static IEnumerable<User> GetFakeData(ParametersForFake parameters)
    {
        var seed = parameters.Seed + parameters.Page;
        var id = parameters.Page * parameters.Amount + 1;

        var testUsers = new Faker<User>(parameters.Locale).UseSeed(seed)
            .StrictMode(true)
            .RuleFor(o => o.Index, f => id++)
            .RuleFor(o => o.Id, f => f.Random.Guid())
            .RuleFor(o => o.FullName, f => f.Name.FindName())
            .RuleFor(o => o.Address, f => f.Address.FullAddress())
            .RuleFor(o => o.Phone, f => f.Phone.PhoneNumber());

        var users = testUsers.Generate(parameters.Amount).ToList();
        return users;
    }
}
