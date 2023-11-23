using Bogus;
using FastEndpoints.Testing;
using Xunit.Abstractions;
using Xunit.Priority;

namespace StockSolution.Api.Tests;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
[Collection("Integration Tests")]
public abstract class TestClass<TFixture> where TFixture : class, IFixture
{
    /// <summary>
    /// fixture data that is shared among all tests of this class
    /// </summary>
    protected TFixture Fixture { get; init; }

#pragma warning disable IDE1006
    /// <summary>
    /// fixture data that is shared among all tests of this class
    /// </summary>
    protected TFixture Fx => Fixture;
#pragma warning restore IDE1006

    /// <summary>
    /// xUnit test output helper
    /// </summary>
    protected ITestOutputHelper Output { get; init; }

    /// <summary>
    /// bogus data generator
    /// </summary>
    protected Faker Fake => Fixture.Fake;

    protected TestClass(TFixture f, ITestOutputHelper o)
    {
        Fixture = f;
        Output = o;
    }
}