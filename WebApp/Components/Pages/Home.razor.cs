using Microsoft.AspNetCore.Components;
using TranslateWebApp.Interfaces;
using TranslateWebApp.Models;
using TransService;

namespace TranslateWebApp.Components.Pages
{
    public partial class Home
    {

        WorkItem? workItem;

        int SelectedItemIndex = -1;
        private bool QueryIsRunning;

        private bool ShowSettings = true;
        private bool IsSaving = false;

        private void MoveFirst()
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

        private async Task MoveNext()
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

        private async Task ApproveAiText()
        {
            statusMessage.Clear();
            IsSaving = true;
            if (workItem != null && !string.IsNullOrEmpty(workItem.WorkAi))
                try
                {
                    await data.ApproveAiText(workItem);
                    await MoveNext();
                }
                catch (Exception e)
                {
                    statusMessage.SetException(e);
                    logger.LogError(e.Message);
                }
            IsSaving = false;
        }

        private async Task ApproveManualText()
        {
            statusMessage.Clear();
            IsSaving = true;
            if (workItem != null && !string.IsNullOrEmpty(workItem.WorkFinal))
                try
                {
                    await data.ApproveText(workItem);
                    await MoveNext();
                }
                catch (Exception e)
                {
                    statusMessage.SetException(e);
                    logger.LogError(e.Message);
                }
            IsSaving = false;
        }

        private async Task RunQuery()
        {
            if (!appUser.Authenticated && QueryIsRunning) return;
            try
            {
                QueryIsRunning = true;
                if (!appUser.Loaded)
                    await data.LoadUserData(appUser.LogTo);
                await data.LoadTranslations();
                MoveFirst();
                statusMessage.Clear();
            }
            catch (Exception e)
            {
                statusMessage.SetException(e);
            }
            QueryIsRunning = false;
            StateHasChanged();
        }

        protected void OnInitialize()
        {
            appUser.Attach(this);
        }

        public void HandleUpdate()
        {
            logger.LogWarning($"{this}.HandleUpdate(): Triggered.");
            StateHasChanged();
        }

    }
}

