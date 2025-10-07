namespace TranslateWebApp.Models
{
    public class UserData
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string LogTo { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string TargetLanguage { get; set; } = "es";
        public string HelperLanguage { get; set; } = "nb";
        public int ProjectId { get; set; }
        public void Clear()
        {
            ProjectId = 4;
            UserId = 0;
            LogTo = string.Empty;
            FirstName = "Public";// string.Empty;
            LastName = "Stranger";//  string.Empty;
            Email = string.Empty;
            TargetLanguage = "es";
            HelperLanguage = "nb";
        }
    }

}
