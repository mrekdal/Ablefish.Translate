using Ablefish.Blazor.Observer;
using System.Security.Claims;

namespace TranslateWebApp.Interfaces
{
    public interface IApplicationUser
    {
        Task SetClaimsPrincipal(ClaimsPrincipal? claimsPrincipal);
        string FirstName { get; }
        string LastName { get; }
        bool Authenticated { get; }
        bool Loaded { get; }
        int ProjectId { get; }
        string LogTo { get; }
        int UserId { get; }
    }
}