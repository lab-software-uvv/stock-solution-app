using System.Net;
using Bogus.Extensions.Brazil;
using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using NodaTime;
using StockSolution.Api.Features.Auth;
using StockSolution.Api.Tests.Helpers;
using Xunit.Abstractions;

namespace StockSolution.Api.Tests.Auth;

public class RegisterTests : TestClass<ApiFixture>
{
    public RegisterTests(ApiFixture f, ITestOutputHelper o) : base(f, o)
    {
    }

    [Theory]
    [MemberData(nameof(TestData.InvalidRegisterData), MemberType = typeof(TestData))]
    public async Task Register_With_Invalid_Input_Should_Return_Bad_Request(RegisterCommand command)
    {
        var (httpResponse, _) =
            await Fixture.Client.POSTAsync<RegisterEndpoint, RegisterCommand, ErrorResponse>(command);

        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_With_Valid_Input_And_Invalid_Invite_Should_Return_NotFound()
    {
        var command = new RegisterCommand("John Doe", "01234567890", new LocalDate(1990, 1, 1), "example@gmail.com",
            "123456", Guid.NewGuid());

        var (httpResponse, _) =
            await Fixture.Client.POSTAsync<RegisterEndpoint, RegisterCommand, ErrorResponse>(command);
        
        httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Register_With_Valid_Input_Should_Register_User()
    {
        var registerEmail = Fixture.Fake.Internet.Email();
        var context = Fixture.CreateDbContext();
        var inviteId = await context.CreateInviteAsync(registerEmail);

        var command = new RegisterCommand("John Doe", Fixture.Fake.Person.Cpf(), new LocalDate(1990, 1, 1), registerEmail,
            "123456", inviteId);

        var (httpResponse, response) =
            await Fixture.Client.POSTAsync<RegisterEndpoint, RegisterCommand, TokenResponse>(command);
        
        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Register_With_Duplicated_Cpf_Should_Return_Conflict()
    {
        var cpf = Fixture.Fake.Person.Cpf();
        var context = Fixture.CreateDbContext();
        
        var firstEmail = Fixture.Fake.Internet.Email();
        var firstInviteId = await context.CreateInviteAsync(firstEmail);
        var firstUserCommand = new RegisterCommand("John Doe", cpf, new LocalDate(1990, 1, 1), firstEmail, "123456", firstInviteId);
        await Fixture.Client.POSTAsync<RegisterEndpoint, RegisterCommand, TokenResponse>(firstUserCommand);
        
        var secondEmail = Fixture.Fake.Internet.Email();
        var secondInviteId = await context.CreateInviteAsync(secondEmail);
        var secondUserCommand = new RegisterCommand("Jane Doe", cpf, new LocalDate(1991, 2, 2), secondEmail, "654321", secondInviteId);
        var (httpResponse, _) = await Fixture.Client.POSTAsync<RegisterEndpoint, RegisterCommand, TokenResponse>(secondUserCommand);
        
        httpResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Register_With_Duplicated_Mail_Should_Return_Conflict()
    {
        var registerEmail = Fixture.Fake.Internet.Email();
        var context = Fixture.CreateDbContext();
        
        var firstInviteId = await context.CreateInviteAsync(registerEmail);
        var firstUserCommand = new RegisterCommand("John Doe", Fake.Person.Cpf(), new LocalDate(1990, 1, 1), registerEmail, "123456", firstInviteId);
        await Fixture.Client.POSTAsync<RegisterEndpoint, RegisterCommand, TokenResponse>(firstUserCommand);
        
        var secondInviteId = await context.CreateInviteAsync(registerEmail);
        var secondUserCommand = new RegisterCommand("Jane Doe", Fixture.Fake.Person.Cpf(), new LocalDate(1991, 2, 2), registerEmail, "654321", secondInviteId);
        var (httpResponse, _) = await Fixture.Client.POSTAsync<RegisterEndpoint, RegisterCommand, TokenResponse>(secondUserCommand);
        
        httpResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}