namespace TranslateWebApp.Models
{
    public class UserData
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string LoadError { get; internal set; } = string.Empty;
        public string LogTo { get; internal set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string TargetLanguage { get; set; } = string.Empty;
        public string HelperLanguage { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public void Clear(string logTo)
        {
            ProjectId = 0;
            UserId = 0;
            LogTo = logTo;
            FirstName = "Public";// string.Empty;
            LastName = "Stranger";//  string.Empty;
            LoadError = string.Empty;   
        }


        public void SetFailed(string logTo, string errorMessage)
        {
            if (logTo != LogTo)
                Clear(logTo);
            LoadError = errorMessage;
        }

        public bool IsValid => UserId > 0;
        public bool IsLoaded => IsValid && string.IsNullOrEmpty(LoadError);

    }

}
