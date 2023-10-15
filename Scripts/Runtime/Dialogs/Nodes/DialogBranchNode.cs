using System;
using System.Collections.Generic;
using DialogSystem.Attributes;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Structure;
using UnityEngine;
using XNode;

namespace DialogSystem.Nodes
{
    public class DialogBranchNode : DialogBaseNode
    {
        public string SelectorTag => _selectorTag;
        [HideInInspector]
        public int SelectIndex {
            get {
                return _selectIndex;
            }
            set {
                _selectIndex = value;
            }
        }
        private int _selectIndex = -1;
        [DialogTagSelector][SerializeField] private string _selectorTag = "Selections";
        [Output(dynamicPortList = true)] public List<DialogContent> Selections = new List<DialogContent>();
        protected override void Init()
        {
            base.Init();
            Type = DialogType.BRANCH;
        }
        public override DialogBaseNode GetNext()
        {
            #if UNITY_EDITOR
                Debug.Log("DialogBranchNode : GetNext / SelectIndex : " + SelectIndex);
            #endif
            NodePort port = GetOutputPort($"Selections {SelectIndex}");
            if (port.ConnectionCount == 0) {
                return null;
            }
            return  port?.GetConnection(0)?.node as DialogBaseNode;
        }
        public override void Play(IDialogManager target) {
            //Debug.Log("DialogBranchNode : Play");
            target.Play(this);
        }
        public override bool IsNextExist() {
            //Debug.Log(" Selections.Count : " + Selections.Count);
            return Selections.Count > 0;
        }
        public override bool CanGetNext() {
            return IsNextExist() &&SelectIndex >= 0 && SelectIndex < Selections.Count;
        }
        public override void ResetNode()
        {
            SelectIndex = -1;
        }
        private void OnValidate()
        {
            if (Selections.Count == 0) Debug.LogWarning("Selections is empty");
        }
    }
}