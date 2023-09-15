using DialogSystem.Runtime.Dialogs.Components.Selections;
using DialogSystem.Structure;
using UnityEngine.EventSystems;

namespace DialogSystem.Runtime.Dialogs.Interfaces
{
    public interface ISelection
    {
        public int SelectionIndex { get; protected set; }
        public DialogSelector ParentSelector { get; protected set; }
        public void Init(int index, DialogContent content, DialogSelector parentSelector);
        public void Show();
        public void Hide();
    }
}