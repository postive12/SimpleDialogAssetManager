using UnityEngine.UIElements;

namespace DialogSystem.Editor.Windows
{
    public class PlotEditorSplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<PlotEditorSplitView, TwoPaneSplitView.UxmlTraits> {}
    }
}