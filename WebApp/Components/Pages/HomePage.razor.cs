using TranslateWebApp.Models;
using TransService;

namespace TranslateWebApp.Components.Pages
{
    public partial class HomePage
    {

        WorkItem? workItem;

        int SelectedItemIndex = -1;
        private bool QueryIsRunning;

        private bool IsSaving = false;

        private bool IsInitializing => appState.CallsTranslations == 0 && QueryIsRunning;

        private void MoveToFirst()
        {
            if (appState.Translations.Count > 0)
            {
                SelectedItemIndex = 0;
                workItem = appState.Translations[0];
            }
            else
            {
                SelectedItemIndex = -1;
                workItem = null;
            }
        }

        private async Task MoveToNext()
        {
            if (SelectedItemIndex < appState.Translations.Count - 1)
            {
                SelectedItemIndex++;
                await SelectItem(SelectedItemIndex);
            }
            else
            {
                await RunQuery();
            }
        }

        private async Task SelectItem(int itemNo)
        {
            if (itemNo >= 0 && itemNo < appState.Translations.Count)
            {
                SelectedItemIndex = itemNo;
                workItem = appState.Translations[SelectedItemIndex];
                await Task.Delay(0);
            }
        }

        private async Task TranslateText(string serviceName)
        {
            statusMessage.Clear();
            if (workItem != null && transFactory.TryGetService(serviceName, out ITransProcessor? service) && service != null)
                try
                {
                    workItem.WorkAi = await service.Translate(workItem.Src1Text, workItem.Src1Key, workItem.LangWorkKey);
                    await data.StoreAiText(workItem, serviceName);

                }
                catch (Exception e)
                {
                    statusMessage.SetException(e);
                }
        }

        private async Task EditText()
        {
            statusMessage.Clear();
            if (workItem != null && !string.IsNullOrEmpty(workItem.WorkAi))
            {
                workItem.WorkFinal = workItem.WorkAi;
            }
            await Task.Delay(0);
        }

        private async Task ApproveAiText(bool withDoubt)
        {
            statusMessage.Clear();
            IsSaving = true;
            if (workItem != null && !string.IsNullOrEmpty(workItem.WorkAi))
                try
                {
                    await data.ApproveAiText(workItem, withDoubt);
                    await MoveToNext();
                }
                catch (Exception e)
                {
                    statusMessage.SetException(e);
                    logger.LogError(e.Message);
                }
            IsSaving = false;
        }

        private async Task ApproveManualText(bool withDoubt)
        {
            statusMessage.Clear();
            IsSaving = true;
            if (workItem != null && !string.IsNullOrEmpty(workItem.WorkFinal))
                try
                {
                    await data.ApproveText(workItem, withDoubt);
                    await MoveToNext();
                }
                catch (Exception e)
                {
                    statusMessage.SetException(e);
                    logger.LogError(e.Message);
                }
            IsSaving = false;
        }


        protected async Task RunQuery()
        {
            if (!appUser.Authenticated && QueryIsRunning) return;
            try
            {
                QueryIsRunning = true;
                if (string.IsNullOrEmpty(SearchFor))
                    await data.LoadTranslations(appUser.LogTo);
                else
                    await data.LoadTranslationsText(appUser.LogTo, SearchFor);
                MoveToFirst();
                statusMessage.Clear();
            }
            catch (Exception e)
            {
                statusMessage.SetException(e);
            }
            QueryIsRunning = false;
            StateHasChanged();
        }


        private async void UserDataLoaded()
        {
            await RunQuery();
        }

        #region Event Handler to register and unregister    
        protected override async Task OnInitializedAsync()
        {
            data.OnUserDataChanged += UserDataLoaded;
            await base.OnInitializedAsync();
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var authState = await AuthStateTask;
                await appUser.SetClaimsPrincipal(authState.User);
                await data.LoadUserData(appUser.LogTo);
            }
        }


        public void Dispose()
        {
            data.OnUserDataChanged -= UserDataLoaded;
        }

        #endregion

    }
}
