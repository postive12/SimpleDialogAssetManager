using DialogSystem.Structure;
using UnityEngine.EventSystems;

namespace DialogSystem.Scripts.Runtime.Dialogs.Selections
{
    public interface ISelection : IPointerClickHandler
    {
        public int SelectionIndex { get; protected set; }
        public DialogSelector ParentSelector { get; protected set; }
        public void Init(int index, DialogContent content, DialogSelector parentSelector);
        public void Show();
        public void Hide();
    }
}