using System;
using DialogSystem.Structure;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DialogSystem.Runtime.Dialogs.Selections
{
    //Add Button component when creating a new DialogSelection
    [RequireComponent(typeof(UnityEngine.UI.Button))]
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
        [Header("Init Events")]
        [SerializeField] private UnityEvent<int> _onInitSelectionIndex;
        [SerializeField] private UnityEvent<string> _onInitSelectionContent;
        [Header("Show/Hide Events")]
        [SerializeField] private UnityEvent _onShowSelectionEvent;
        [SerializeField] private UnityEvent _onHideSelectionEvent;
        private DialogSelector _parentSelector;
        private int _selectionIndex = 0;
        private DialogSelector _parentSelector1;
        private void Start()
        {
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                _parentSelector.Select(_selectionIndex);
            });
        }
        public void Init(int index ,DialogContent content, DialogSelector parentSelector)
        {
            _selectionIndex = index;
            _parentSelector = parentSelector;
            _onInitSelectionIndex?.Invoke(index);
            _onInitSelectionContent?.Invoke(content.Content);
        }
        public void Show() {
            //If there is no show event, then activate the gameobject
            if (_onShowSelectionEvent.GetPersistentEventCount() <= 0) {
                gameObject.SetActive(true);
                return;
            }
            _onShowSelectionEvent?.Invoke();
        }
        public void Hide()
        {
            //If there is no hide event, then deactivate the gameobject
            if (_onHideSelectionEvent.GetPersistentEventCount() <= 0) {
                gameObject.SetActive(false);
                return;
            }
            _onHideSelectionEvent?.Invoke();
        }
    }
}