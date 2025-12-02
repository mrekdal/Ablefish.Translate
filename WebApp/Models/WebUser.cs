using System.Security.Claims;
using Ablefish.Blazor.Observer;
using TranslateWebApp.Interfaces;

namespace TranslateWebApp.Models
{
    public class WebUser : IApplicationUser
    {

        private IDataContext _dataContext;
        private string _logTo = string.Empty;

        public WebUser(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public int UserId { get => _dataContext.UserData.UserId; }
        public int ProjectId { get => _dataContext.UserData.ProjectId; }
        public string FirstName { get => _dataContext.UserData.FirstName; }
        public string LastName { get => _dataContext.UserData.LastName; }
        public bool Authenticated { get => User?.Identity?.IsAuthenticated ?? false; }
        public bool IsValid { get => _dataContext.UserData.UserId > 0; }

        public string LogTo { get => User?.FindFirst("sub")?.Value ?? ""; }

        public ClaimsPrincipal? User { get; internal set; }

        public async Task SetClaimsPrincipal(ClaimsPrincipal? claimsPrincipal)
        {
            User = claimsPrincipal;
        }

    }
}
