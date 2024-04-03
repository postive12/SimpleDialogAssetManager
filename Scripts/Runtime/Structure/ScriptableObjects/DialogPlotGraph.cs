using System;
using System.Collections.Generic;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Nodes;
using DialogSystem.Nodes.Branches;
using DialogSystem.Nodes.Lines;
using DialogSystem.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace DialogSystem.Runtime.Structure.ScriptableObjects
{
    [CreateAssetMenu(menuName = "DialogSystem/DialogPlotGraph", fileName = "DialogPlotGraph")]
    public class DialogPlotGraph : DialogScriptableObject
    {
        public int Length => Nodes.Count;
        public DialogBaseNode CurrentNode { get; private set; } = null;
        [HideInInspector] public bool IsPlotEnd = true;
        [HideInInspector] public DialogBaseNode StartNode = null;
        //Need to remake start and end point
        public List<DialogBaseNode> Nodes = new List<DialogBaseNode>();
        public DialogPlotGraph() {
            _plotDataType = PlotDataType.PLOT;
        }
        public void PlayPlot() {
            IsPlotEnd = false;
            CurrentNode = StartNode;
        }
        public void Next(IDialogManager target) {
            if (IsPlotEnd) {
                return;
            }
            if (CurrentNode.CanGetNext()) {
                var lastNode = CurrentNode;
                var nextNode = CurrentNode.GetNext();
                lastNode.ResetNode();
                nextNode.ResetNode();
                CurrentNode = nextNode;
                CurrentNode.Play(target);
            }
            //If can load next node, load next node
            if (!CurrentNode.IsNextExist()) {
                //Debug.LogWarning("Can't go next");
                IsPlotEnd = true;
                CurrentNode = null;
                return;
            }
        }
        protected override void OnValidate() {
            base.OnValidate();
        }

        #region Editor
        #if UNITY_EDITOR
        public DialogBaseNode CreateNode(Type type)
        {
            var node = CreateInstance(type) as DialogBaseNode;
            if (node == null) return null;
            node.name = type.Name;
            Nodes.Add(node);
            node.OnValidate();
            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
            return node;
        }
        public void DeleteNode(DialogBaseNode node)
        {
            Nodes.Remove(node);
            foreach (var n in Nodes) {
                n.OnValidate();
            }
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }
        public void AddChild(DialogBaseNode parent, DialogBaseNode child)
        {
            DialogBaseNode root = parent as DialogBaseNode;
            
            DialogStartNode start = parent as DialogStartNode;
            if (start != null) {
                start.Child = child;
            }
            SingleChildNode single = parent as SingleChildNode;
            if (single != null) {
                single.Child = child;
            }
            MultipleChildNode multiple = parent as MultipleChildNode;
            if (multiple != null) {
                multiple.Children.Add(child);
            }
            parent.OnValidate();
        }
        public void RemoveChild(DialogBaseNode parent, DialogBaseNode child)
        {
            DialogBaseNode root = parent as DialogBaseNode;
            DialogStartNode start = parent as DialogStartNode;
            if (start != null) {
                start.Child = null;
            }
            SingleChildNode single = parent as SingleChildNode;
            if (single != null) {
                single.Child = null;
            }
            MultipleChildNode multiple = parent as MultipleChildNode;
            if (multiple != null) {
                multiple.Children.Remove(child);
            }
            parent.OnValidate();
        }
        public List<DialogBaseNode> GetChildren(DialogBaseNode parent)
        {
            List<DialogBaseNode> children = new List<DialogBaseNode>();
            if (parent is DialogStartNode start) {
                if (start.Child != null) {
                    children.Add(start.Child);
                }
            }
            if (parent is SingleChildNode single) {
                if (single.Child != null) {
                    children.Add(single.Child);
                }
            }
            if (parent is MultipleChildNode multi) {
                children.AddRange(multi.Children);
            }
            return children;
        }
        #endif
        #endregion
    }
}