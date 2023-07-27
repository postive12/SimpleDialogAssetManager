using System.Collections.Generic;
using DialogSystem.Attributes;
using DialogSystem.Nodes;
using DialogSystem.Structure;
using UnityEngine;

namespace DialogSystem.Runtime.Dialogs.Selections
{
    public class DialogSelector : MonoBehaviour,ISelector
    {
        string ISelector.SelectorTag {
            get {
                return _selectorTag;
            }
            set {
                _selectorTag = value;
            }
        }
        [DialogTagSelector][SerializeField] private string _selectorTag = "NONE";
        [SerializeField] private GameObject _selectionPrefab = null;
        [SerializeField] private List<DialogSelection> _selectionComponents = new List<DialogSelection>();
        private DialogBranchNode _targetNode = null;
        private void Awake()
        {
            DialogManager.AddSelector(this);
        }
        public void CreateSelections(List<DialogContent> selections, DialogBranchNode node)
        {
            _targetNode = node;
            int count = 0;
            for (count = 0; count  < selections.Count && count < _selectionComponents.Count; count++) {
                _selectionComponents[count].Init(count,selections[count],this);
                _selectionComponents[count].Show();
            }
            for (; count < selections.Count; count++) {
                var selection = Instantiate(_selectionPrefab, transform).GetComponent<DialogSelection>();
                selection.Init(count,selections[count],this);
                selection.Show();
                _selectionComponents.Add(selection);
            }
            for (; count < _selectionComponents.Count; count++) {
                _selectionComponents[count].Hide();
            }
        }
        public void HideSelections()
        {
            foreach (var dialogSelection in _selectionComponents) {
                dialogSelection.Hide();
            }
        }
        public void Select(int index)
        {
            if(_targetNode == null) return;
            _targetNode.SelectIndex = index;
            _targetNode = null;
            HideSelections();
            DialogManager.Instance.RequestDialog();
        }
    }
}