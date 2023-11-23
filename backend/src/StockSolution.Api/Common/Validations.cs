namespace StockSolution.Api.Common;

public static class Validations
{
    public static bool BeValidCpf(string? cpf)
    {
        cpf = cpf?.Where(char.IsDigit).Aggregate("", (current, c) => current + c);
        if (cpf is null || cpf.Length != 11 || cpf.ContainsOnlyIdenticalCharacters())
            return false;
        
        var sumForFirstDigit = 0;
        var sumForSecondDigit = 0;

        for (var i = 0; i < 9; i++)
        {
            sumForFirstDigit += (cpf[i] - '0') * (10 - i);
            sumForSecondDigit += (cpf[i] - '0') * (11 - i);
        }

        var firstDigit = sumForFirstDigit * 10 % 11;
        firstDigit = firstDigit == 10 ? 0 : firstDigit;

        sumForSecondDigit += firstDigit * 2;
        var secondDigit = sumForSecondDigit * 10 % 11;
        secondDigit = secondDigit == 10 ? 0 : secondDigit;

        return cpf.EndsWith($"{firstDigit}{secondDigit}");
    }
    
    public static bool BeValidGuid(Guid guid)
    {
        return guid != Guid.Empty;
    }
    
    private static bool ContainsOnlyIdenticalCharacters(this string input)
        => input.Distinct().Count() == 1;
}