using System.Security.Claims;

namespace TranslateWebApp.Interfaces
{
    public interface IWebUser
    {
        Task SetClaimsPrincipal(ClaimsPrincipal? claimsPrincipal);
        Task SetLogTo(string? logTo);
        string FirstName { get; }
        string LastName { get; }
        bool IsAuthenticated { get; }
        int ProjectId { get; }
        string LogTo { get; }
        int UserId { get; }
    }
}