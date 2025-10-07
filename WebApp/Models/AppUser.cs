using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using TranslateWebApp.Data;

namespace TranslateWebApp.Models
{
    public class AppUser : IAppUser
    {

        private IDataContext _dataContext;
        private UserData _userData = new();
        private string _logTo = string.Empty;

        public AppUser(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public int UserId { get => _userData.UserId; }
        public int ProjectId { get => _userData.ProjectId; }
        public string FirstName { get => _userData.FirstName; }
        public string LastName { get => _userData.LastName; }
        public bool IsAuthenticated { get => ( _userData.UserId > 0 && ( User?.Identity?.IsAuthenticated ?? false ) ); }

        public string TargetLanguage { get => _dataContext.TargetLanguage; set => _dataContext.TargetLanguage = value; }
        public string HelperLanguage { get => _dataContext.HelperLanguage; set => _dataContext.HelperLanguage = value; }

        public string LogTo { get => _userData.LogTo; }

        public ClaimsPrincipal? User { get; internal set; }

        public async Task SetLogTo(string? logTo)
        {
            if (_userData.LogTo == logTo) return;
            if (string.IsNullOrWhiteSpace(logTo?.Trim()))
                _userData.Clear();
            else
                _userData = await _dataContext.GetUserData(logTo);
        }

        public async Task SetClaimsPrincipal(ClaimsPrincipal? claimsPrincipal)
        {
            User = claimsPrincipal;
            await SetLogTo(claimsPrincipal?.FindFirst("sub")?.Value);
            // Notify();
        }

    }
}
