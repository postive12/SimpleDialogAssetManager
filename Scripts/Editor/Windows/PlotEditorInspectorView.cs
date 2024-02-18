using DialogSystem.Nodes;
using UnityEngine.UIElements;

namespace DialogSystem.Editor.Windows
{
    public class PlotEditorInspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<PlotEditorInspectorView, VisualElement.UxmlTraits> {}
        private UnityEditor.Editor _editor;
        public PlotEditorInspectorView()
        {
        }

        internal void UpdateSelection(DLNodeView node)
        {
            Clear();
            UnityEngine.Object.DestroyImmediate(_editor);
            _editor = UnityEditor.Editor.CreateEditor(node.Node);
            IMGUIContainer container = new IMGUIContainer(() => { _editor.OnInspectorGUI(); });
            Add(container);
        }
    }
}