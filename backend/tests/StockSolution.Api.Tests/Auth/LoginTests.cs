using System.Net;
using FastEndpoints;
using FluentAssertions;
using StockSolution.Api.Features.Auth;
using StockSolution.Api.Persistence;
using Xunit.Abstractions;

namespace StockSolution.Api.Tests.Auth;

public class LoginTests : TestClass<ApiFixture>
{
    public LoginTests(ApiFixture f, ITestOutputHelper o) : base(f, o)
    {
    }
    
    [Theory]
    [InlineData("admin", "123", 2)]
    [InlineData(null, "123", 2)]
    [InlineData("admin", null, 2)]
    [InlineData("admin@gmail.com", "123", 1)]
    [InlineData("admin", "1234567", 1)]
    public async Task Login_With_Invalid_Input_Should_Return_Bad_Request(string login, string password, int expectedErrorCount)
    {
        var command = new LoginCommand(login, password);
        var (httpResponse, errorResponse) = await Fixture.Client.POSTAsync<LoginEndpoint, LoginCommand, ErrorResponse>(command);

        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errorResponse.Errors.Should().HaveCount(expectedErrorCount);
    }

    [Fact]
    public async Task Login_With_Invalid_Credentials_Should_Return_Unauthorized()
    {
        var command = new LoginCommand(Fake.Internet.Email(), Fixture.Fake.Internet.Password());
        var (httpResponse, _) = await Fixture.Client.POSTAsync<LoginEndpoint, LoginCommand, ErrorResponse>(command);
        httpResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Login_With_Valid_Credentials_Should_Return_Token()
    {
        var command = new LoginCommand(DataFactory.AdminLogin, DataFactory.AdminPassword);
        var (httpResponse, rsp) = await Fixture.Client.POSTAsync<LoginEndpoint, LoginCommand, TokenResponse>(command);
        
        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        rsp.Token.Should().NotBeNullOrEmpty();
    }
}