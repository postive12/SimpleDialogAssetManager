using UnityEngine.UIElements;

namespace DialogSystem.Editor.CustomEditors
{
    public class DialogEditorSplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<DialogEditorSplitView, TwoPaneSplitView.UxmlTraits> {}
    }
}