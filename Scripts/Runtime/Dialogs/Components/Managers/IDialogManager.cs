using DialogSystem.Nodes;

namespace DialogSystem.Dialogs.Components.Managers
{
    public interface IDialogManager
    {
        void SelectDialogPlot(string eventName);
        void RequestDialog();
        void Play(DialogNode node);
        void Play(DialogBranchNode node);
    }
}