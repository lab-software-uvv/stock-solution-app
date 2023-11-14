using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace StockSolution.Api.Common.Exceptions;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var errorMessage = new { Erro = "" };
        string jsonResponse;

        if (exception is Microsoft.EntityFrameworkCore.DbUpdateException dbUpdateException)
        {
            var sqlException = (dbUpdateException.InnerException as PostgresException)!;

            if (IsUniqueIndexViolationError(sqlException))
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                context.Response.ContentType = "application/json";
                errorMessage = new { Erro = $"Conflito: Durante o Processo Ocorreu a Violação da Chave Única {sqlException.ConstraintName}." };

                jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(errorMessage, Newtonsoft.Json.Formatting.Indented);
                await context.Response.WriteAsync(jsonResponse);
                return;
            }
        }
        else if (exception.InnerException.Message.Equals("The given key was not present in the dictionary."))
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/json";
            errorMessage = new { Erro = $"Não Encontrado: {exception.Message}." };

            jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(errorMessage, Newtonsoft.Json.Formatting.Indented);
            await context.Response.WriteAsync(jsonResponse);
            return;

        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        errorMessage = new { Erro = "Um Erro Interno do Servidor Ocorreu. "};

        jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(errorMessage, Newtonsoft.Json.Formatting.Indented);
        await context.Response.WriteAsync(jsonResponse);
    }

    private bool IsUniqueIndexViolationError(PostgresException ex)
    {
        if (ex!.SqlState == "23505")
            return true;
        else
            return false;
    }
}
