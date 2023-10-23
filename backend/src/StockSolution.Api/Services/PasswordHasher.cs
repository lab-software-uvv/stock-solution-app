using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace StockSolution.Api.Services;

[RegisterScoped]
public class PasswordHasher
{
    private const int SaltSize = 128 / 8;
    private const int KeySize = 256 / 8;
    private const int Iterations = 10000;
    private const KeyDerivationPrf Algorithm = KeyDerivationPrf.HMACSHA256;
    private const char Delimiter = '.';

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = KeyDerivation.Pbkdf2(password, salt, Algorithm, Iterations, KeySize);
        return $"{Convert.ToBase64String(salt)}{Delimiter}{Convert.ToBase64String(hash)}";
    }

    public bool Verify(string passwordHash, string inputPassword)
    {
        var parts = passwordHash.Split(Delimiter);
        var salt = Convert.FromBase64String(parts[0]);
        var hash = Convert.FromBase64String(parts[1]);

        var newHash = KeyDerivation.Pbkdf2(inputPassword, salt, Algorithm, Iterations, KeySize);
        return CryptographicOperations.FixedTimeEquals(hash, newHash);
    }
}