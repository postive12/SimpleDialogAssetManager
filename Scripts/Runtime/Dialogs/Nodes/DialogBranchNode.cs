using System;
using System.Collections.Generic;
using DialogSystem.Attributes;
using DialogSystem.Structure;
using UnityEngine;
using XNode;

namespace DialogSystem.Nodes
{
    public class DialogBranchNode : DialogBaseNode
    {
        public string SelectorTag => _selectorTag;
        [HideInInspector]public int SelectIndex = -1;
        [DialogTagSelector][SerializeField] private string _selectorTag = "Selections";
        [Output(dynamicPortList = true)] public List<DialogContent> Selections = new List<DialogContent>();
        protected override void Init()
        {
            base.Init();
            Type = DialogType.BRANCH;
        }
        public override DialogBaseNode GetNext()
        {
            NodePort port = GetOutputPort($"Selections {SelectIndex}");
            ResetNode();
            if (port.ConnectionCount == 0) {
                return null;
            }
            return  port?.GetConnection(0)?.node as DialogBaseNode;
        }
        public override bool CanGoNext()
        {
            return SelectIndex >= 0 && SelectIndex < Selections.Count;
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