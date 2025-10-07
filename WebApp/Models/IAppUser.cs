using System.Security.Claims;

namespace TranslateWebApp.Models
{
    public interface IAppUser
    {
        Task SetClaimsPrincipal(ClaimsPrincipal? claimsPrincipal);
        Task SetLogTo(string? logTo);

        string FirstName { get; }
        bool IsAuthenticated { get; }
        int ProjectId { get; }
        string LogTo { get; }
        int UserId { get; }
    }
}