using TranslateWebApp.Models;

namespace TranslateWebApp.Components.Pages
{
    public partial class Conflicts
    {
        private TextConflict? textConflict;
        private int SelectedItemIndex;
        private bool ShowNumbers = false;
        private bool QueryIsRunning = false;
        private int rowNo = 0;


        private async Task ClickStart()
        {
            // StartClicked = true;
            await RunQuery();
        }

        private void PickRow(int rowNo)
        {
            textConflict = appState.Conflicts.Items[rowNo];
        }

        private void PickFirst()
        {
            if (appState.Conflicts.Items.Count > 0)
            {
                SelectedItemIndex = 0;
                textConflict = appState.Conflicts.Items[0];
                statusMessageQuery.Clear();
            }
            else
            {
                statusMessageQuery.SetSuccess("No more conflicts", "There are no translation conflicts for the selected project and language");
                SelectedItemIndex = -1;
                textConflict = null;
            }
        }

        private async Task PickBlock(int blockId)
        {
            statusMessageApprove.Clear();
            if (textConflict == null) return;
            foreach (var c in textConflict.Candidate)
                if (c.BlockId == blockId)
                    c.Approved = true;
                else if (c.BlockId != blockId && !c.Discarded)
                    await DiscardBlock(c.BlockId);
        }

        private async Task DiscardBlock(int blockId)
        {
            statusMessageApprove.Clear();
            try
            {
                if (textConflict == null) return;
                await data.DiscardBlock(blockId);
                if (textConflict.DiscardBlock(blockId) == 0)
                    PickFirst();
            }
            catch (Exception e)
            {
                statusMessageApprove.SetException(e);
            }
        }

        private async Task RunQuery()
        {
            if (!appUser.Authenticated || QueryIsRunning) return;
            try
            {
                statusMessageQuery.SetInformation("Loading data ...", "Retrieving a list of conflicts for you to resolve.  Please wait for this message to disappear.");
                QueryIsRunning = true;
                if (!appUser.Loaded)
                    await data.LoadUserData(appUser.LogTo);
                await data.LoadConflicts();
                PickFirst();
                statusMessageQuery.Clear();
            }
            catch (Exception e)
            {
                statusMessageQuery.SetException(e);
            }
            QueryIsRunning = false;
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if ( appState.CallsConflicts == 0 && appUser.Authenticated)
                await RunQuery();
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
