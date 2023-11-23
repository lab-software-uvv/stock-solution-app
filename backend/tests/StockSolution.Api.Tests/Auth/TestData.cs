using NodaTime;
using StockSolution.Api.Features.Auth;

namespace StockSolution.Api.Tests.Auth;

public static class TestData
{
    public static IEnumerable<object[]> InvalidRegisterData => new List<object[]>
    {
        // All fields are empty
        new object[] { new RegisterCommand(default!, default!, default, default!, default!, default) },

        // Name is less than 6 characters
        new object[]
        {
            new RegisterCommand("John", "01234567890", new LocalDate(1990, 1, 1), "john.doe@example.com", "123456",
                Guid.NewGuid())
        },

        // Email is empty
        new object[]
        {
            new RegisterCommand("Abdullah", "01234567890", new LocalDate(1990, 1, 1), default!, "123456",
                Guid.NewGuid())
        },

        // Email is not valid
        new object[]
        {
            new RegisterCommand("Abdullah", "01234567890", new LocalDate(1990, 1, 1), "invalid_email", "123456",
                Guid.NewGuid())
        },

        // Password is empty
        new object[]
        {
            new RegisterCommand("Abdullah", "01234567890", new LocalDate(1990, 1, 1), "john.doe@example.com", default!,
                Guid.NewGuid())
        },

        // Password is less than 5 characters
        new object[]
        {
            new RegisterCommand("Abdullah", "01234567890", new LocalDate(1990, 1, 1), "john.doe@example.com", "1234",
                Guid.NewGuid())
        },

        // CPF is empty
        new object[]
        {
            new RegisterCommand("Abdullah", default!, new LocalDate(1990, 1, 1), "john.doe@example.com", "123456",
                Guid.NewGuid())
        },

        // CPF is not valid (assuming 'Validations.BeValidCpf' is a specific CPF format validator)
        new object[]
        {
            new RegisterCommand("Abdullah", "invalid_cpf", new LocalDate(1990, 1, 1), "john.doe@example.com", "123456",
                Guid.NewGuid())
        },

        // BirthDate is empty
        new object[]
        {
            new RegisterCommand("Abdullah", "01234567890", default, "john.doe@example.com", "123456", Guid.NewGuid())
        },

        // BirthDate is not a valid date (e.g., future date)
        new object[]
        {
            new RegisterCommand("Abdullah", "01234567890", LocalDate.MaxIsoValue, "john.doe@example.com", "123456",
                Guid.NewGuid())
        },

        // BirthDate is not in the past
        new object[]
        {
            new RegisterCommand("Abdullah", "01234567890", LocalDate.FromDateTime(DateTime.Now.AddDays(1)),
                "john.doe@example.com", "123456", Guid.NewGuid())
        },

        // Invite code is empty
        new object[]
        {
            new RegisterCommand("Abdullah", "01234567890", new LocalDate(1990, 1, 1), "john.doe@example.com", "123456",
                default)
        },

        // Invite code is not valid (assuming 'Validations.BeValidGuid' checks for non-empty and correctly formatted GUIDs)
        new object[]
        {
            new RegisterCommand("Abdullah", "01234567890", new LocalDate(1990, 1, 1), "john.doe@example.com", "123456",
                Guid.Empty)
        }
    };
}