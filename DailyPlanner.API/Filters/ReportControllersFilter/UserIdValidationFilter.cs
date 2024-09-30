using System.Security.Claims;
using DailyPlanner.API.Extensions;
using DailyPlanner.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace DailyPlanner.API.Filters.ReportControllersFilter;

/// <summary>
/// Проверяет, равен ли id пользователя переданному userId.
/// </summary>
public class UserIdValidationFilter : ValidatorFilterBase, IAsyncAuthorizationFilter
{
    /// <summary>
    /// Получает имя идентификатора для сравнения с тем же идентификатором пользователя.
    /// </summary>
    /// <param name="identifierName">Имя идентификатора для сравнения с тем же идентификатором пользователя</param>
    public UserIdValidationFilter(string? identifierName = null)
    {
        IdentifierName = identifierName ?? "userId";
    }

    /// <inheritdoc />
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (userId == null)
        {
            context.HttpContext.Request.EnableBuffering();
            var bodyStream = new StreamReader(context.HttpContext.Request.Body);
            var bodyText = await bodyStream.ReadToEndAsync();
            context.HttpContext.Request.Body.Position = 0;

            dynamic requestBody = JsonConvert.DeserializeObject(bodyText);
            userId = (requestBody?[IdentifierName] ?? requestBody?.UserId)?.ToString();
        }

        userId.ThrowIfNullWithReferenceObj(IdentifierName);

        var userRole = user.FindAll(ClaimTypes.Role);
        string[] canGetAnyDataRoles =
        [
            nameof(Roles.Admin),
            nameof(Roles.Moderator)
        ];
        var canGetAnyData = userRole.Any(currentRole => canGetAnyDataRoles.Any(role => role == currentRole.Value));
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var isSame = id == userId;
        if (!(canGetAnyData || isSame))
        {
            context.Result = new ForbidResult(AuthenticationScheme);
        }
    }
}