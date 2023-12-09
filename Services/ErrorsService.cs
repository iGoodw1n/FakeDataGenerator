using Bogus;
using FakeUserDataGenerator.Models;

namespace FakeUserDataGenerator.Services;

public class ErrorsService
{
    private readonly Randomizer _random;
    private readonly string _locale;
    public ErrorsService(int seed, string locale)
    {
        _random = new Randomizer(seed);
        _locale = locale;
    }
    public void CorruptData(IEnumerable<User> users, float errors)
    {
        foreach (var user in users)
        {
            var errorForUser = CalculateNumberOfErrors(errors);
            AddErrorsFor(user, errorForUser);
        }
    }

    private void AddErrorsFor(User user, int errorForUser)
    {
        var (errorsForAddress, errorsForName, errorsForNumber) = CalculateErrorsForUser(errorForUser);

        user.FullName = AddErrorsFor(user.FullName, errorsForAddress);
        user.Address = AddErrorsFor(user.Address, errorsForName);
        user.Phone = AddErrorsFor(user.Phone, errorsForNumber);
    }

    private static (int AddressErrors, int NameErrors, int PhoneErrors) CalculateErrorsForUser(int errorForUser)
    {
        var errorsForAddress = (int)Math.Round(errorForUser * 0.6);
        var errorsForName = (int)Math.Round(errorForUser * 0.2);
        var errorsForPhone = errorForUser - errorsForAddress - errorsForName;
        return (errorsForAddress, errorsForName, errorsForPhone);
    }

    private string AddErrorsFor(string text, int errors)
    {
        var dataForErrors = new DataForErrors(text, errors, _random);
        dataForErrors.AddWildCardsForReplace().SwapChars().RemoveChars();
        return GetResultString(dataForErrors);
    }

    private int CalculateNumberOfErrors(float errors)
    {
        var errorsForUser = (int)Math.Truncate(errors);

        var fractional = errors % 1;
        if (fractional != 0)
        {
            errorsForUser += _random.Float() <= fractional ? 1 : 0;
        }

        return errorsForUser;
    }

    private string GetResultString(DataForErrors dataForErrors)
    {
        var textWithErrors = new string(dataForErrors.Symbols.Where(c => c != '\0').ToArray());
        return ReplaceStars(textWithErrors);
    }

    private string ReplaceStars(string text)
    {
        var faker = new Faker(_locale);
        faker.Random = _random;
        return _random.ReplaceSymbols(text, '*', () =>
        {
            return _random.Float() > 0.5 ? (char)(_random.Number(9) + '0') : faker.Lorem.Letter()[0];
        });
    }
}
