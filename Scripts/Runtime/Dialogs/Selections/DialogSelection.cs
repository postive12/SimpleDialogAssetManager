using DialogSystem.Structure;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DialogSystem.Scripts.Runtime.Dialogs.Selections
{
    public class DialogSelection : MonoBehaviour, ISelection
    {
        int ISelection.SelectionIndex {
            get {
                return _selectionIndex;
            }
            set {
                _selectionIndex = value;
            }
        }
        DialogSelector ISelection.ParentSelector {
            get {
                return _parentSelector1;
            }
            set {
                _parentSelector1 = value;
            }
        }
        [SerializeField] private DialogSelector _parentSelector;
        [Header("Init Events")]
        [SerializeField] private UnityEvent<int> _onInitSelectionIndex;
        [SerializeField] private UnityEvent<string> _onInitSelectionContent;
        [Header("Show/Hide Events")]
        [SerializeField] private UnityEvent _onShowSelectionEvent;
        [SerializeField] private UnityEvent _onHideSelectionEvent;
        private int _selectionIndex = 0;
        private DialogSelector _parentSelector1;
        public void Init(int index ,DialogContent content, DialogSelector parentSelector)
        {
            _selectionIndex = index;
            _parentSelector = parentSelector;
            _onInitSelectionIndex?.Invoke(index);
            _onInitSelectionContent?.Invoke(content.Content);
        }
        public void Show() {
            //If there is no show event, then activate the gameobject
            if (_onShowSelectionEvent == null) {
                gameObject.SetActive(true);
                return;
            }
            _onShowSelectionEvent?.Invoke();
        }
        public void Hide()
        {
            //If there is no hide event, then deactivate the gameobject
            if (_onHideSelectionEvent == null) {
                gameObject.SetActive(false);
                return;
            }
            _onHideSelectionEvent?.Invoke();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            _parentSelector.Select(_selectionIndex);
        }
    }
}