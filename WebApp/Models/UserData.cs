namespace TranslateWebApp.Models
{
    public class UserData
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string LogTo { get; internal set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool Loaded { get; internal set; }
        public string TargetLanguage { get; set; } = string.Empty;
        public string HelperLanguage { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public void Clear(string logTo)
        {
            Loaded = false;
            ProjectId = 0;
            UserId = 0;
            LogTo = logTo;
            FirstName = "Public";// string.Empty;
            LastName = "Stranger";//  string.Empty;
        }

        public void SetLoaded()
        {
            Loaded = true;
        }
    }

}
