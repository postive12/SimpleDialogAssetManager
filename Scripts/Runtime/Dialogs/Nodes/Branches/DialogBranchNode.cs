using System;
using System.Collections.Generic;
using DialogSystem.Runtime.Attributes;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using UnityEngine;

namespace DialogSystem.Nodes.Branches
{
    public class DialogBranchNode : MultipleChildNode
    {
        public string SelectorTag => _selectorTag;
        public int SelectIndex {
            get {
                return _selectIndex;
            }
            set {
                _selectIndex = value;
            }
        }
        public List<DialogContent> Selections => _selections;
        private int _selectIndex = -1;
        [DialogTagSelector]
        [SerializeField] private string _selectorTag = "Selections";
        [SerializeField] private List<DialogContent> _selections = new List<DialogContent>();
        public DialogBranchNode() {
            Type = DialogType.BRANCH;
        }
        public override DialogBaseNode GetNext()
        {
            #if UNITY_EDITOR
                Debug.Log("DialogBranchNode : GetNext / SelectIndex : " + SelectIndex);
            #endif
            if (SelectIndex < 0 || SelectIndex >= Children.Count) {
                return null;
            }
            return Children[SelectIndex];
        }
        public override void Play(IDialogManager target) {
            //Debug.Log("DialogBranchNode : Play");
            target.Play(this);
        }
        public override bool IsNextExist() {
            //Debug.Log(" Selections.Count : " + Selections.Count);
            return Children.Count > 0;
        }
        public override bool CanGetNext() {
            return IsNextExist() && SelectIndex >= 0 && SelectIndex < Children.Count;
        }
        public override void ResetNode()
        {
            SelectIndex = -1;
        }
        private void OnValidate()
        {
            if (Children.Count == 0) Debug.LogWarning("Selections is empty");
        }

        public override void CheckIntegrity()
        {
            #if HAS_LOCALIZATION
            if (_selections.Count != Children.Count) {
                Debug.LogWarning("Selection  is not equal");
                for (int i = _selections.Count; i < Children.Count; i++) {
                    _selections.Add(new DialogContent());
                }
            }
            #endif
        }
    }
}