namespace StockSolution.Api.Features.Events;

public sealed class EmployeesRolesGroup : Group
{
    public EmployeesRolesGroup()
    {
        Configure("EmployeesRole", ep => ep.Description(b => b.Produces(401)));
    }
}