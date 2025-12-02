namespace TranslateWebApp.Models
{
    public enum UserDataLoadStatus
    {
        NotLoaded,
        Cleared,
        Loaded,
        LoadFailed
    }
    public class UserData
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string LoadError { get; internal set; } = string.Empty;
        public string LogTo { get; internal set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserDataLoadStatus LoadStatus { get; internal set; }
        public string TargetLanguage { get; set; } = string.Empty;
        public string HelperLanguage { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public void Clear(string logTo)
        {
            LoadStatus = UserDataLoadStatus.Cleared;
            ProjectId = 0;
            UserId = 0;
            LogTo = logTo;
            FirstName = "Public";// string.Empty;
            LastName = "Stranger";//  string.Empty;
        }

        public void SetLoaded()
        {
            LoadStatus = UserDataLoadStatus.Loaded;
        }

        public void SetFailed(string logTo, string errorMessage)
        {
            if (logTo != LogTo)
                Clear(logTo);
            LoadStatus = UserDataLoadStatus.LoadFailed;
            LoadError = errorMessage;
        }

        public bool MustLoad => LoadStatus == UserDataLoadStatus.NotLoaded || LoadStatus == UserDataLoadStatus.Cleared;
        public bool IsLoaded => !MustLoad;
        public bool IsValid => LoadStatus == UserDataLoadStatus.Loaded && UserId > 0;

    }

}
