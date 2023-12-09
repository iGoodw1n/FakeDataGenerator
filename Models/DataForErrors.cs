using Bogus;

namespace FakeUserDataGenerator.Models;

public class DataForErrors
{
    private readonly Randomizer _random;
    public DataForErrors(string text, int totalErrors, Randomizer random)
    {
        _random = random;
        TotalErrors = totalErrors;
        RemainingErrors = TotalErrors;
        Symbols = text.ToCharArray();
    }

    public char[] Symbols { get; private set; }
    public int TotalErrors { get; }
    public int RemainingErrors { get; private set; }

    public DataForErrors AddWildCardsForReplace()
    {
        var adds = CalcErrors();
        var current = Symbols;
        Symbols = new char[Symbols.Length + adds];

        SetCurrentChars(current);

        for (int i = 0; i < Symbols.Length; i++)
        {
            if (Symbols[i] == default)
            {
                Symbols[i] = '*';
            }
        }
        RemainingErrors -= adds;
        return this;
    }

    public DataForErrors SwapChars()
    {
        int swaps = CalcErrors();
        for (int i = 0; i < swaps; i++)
        {
            SwapRandomChars();
        }
        RemainingErrors -= swaps;
        return this;
    }

    public DataForErrors RemoveChars()
    {
        for (int i = 0; i < RemainingErrors; i++)
        {
            RemoveRandomChar();
        }

        return this;
    }

    private void SetCurrentChars(char[] chars)
    {
        var probability = chars.Length * 1F / Symbols.Length;

        var pt = 0;
        for (int i = 0; i < Symbols.Length; i++)
        {
            if (Symbols.Length - i == chars.Length - pt)
            {
                probability = 2;
            }

            Symbols[i] = _random.Float() <= probability ? chars[pt++] : '\0';
            if (pt >= chars.Length)
            {
                return;
            }
        }
    }

    private int CalcErrors()
    {
        var errors = (int)Math.Round(TotalErrors * 0.33);
        errors = Math.Min(errors, RemainingErrors);
        return errors;
    }

    private void RemoveRandomChar()
    {
        var position = _random.Number(Symbols.Length - 1);
        Symbols[position] = '\0';
    }

    private void SwapRandomChars()
    {
        var position = _random.Number(Symbols.Length - 2);
        (Symbols[position + 1], Symbols[position]) = (Symbols[position], Symbols[position + 1]);
    }
}
