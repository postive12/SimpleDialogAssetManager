using System;
using System.Collections;
using System.Collections.Generic;
using DialogSystem.Runtime;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using DialogSystem.Runtime.Structure.ScriptableObjects.Interface;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogSystem.Editor.CustomEditors.PlotEditors
{
    public class PlotEditorTreeView : TreeView
    {
        private static TreeViewItemData<DialogScriptableObject> GetTreeViewItems(ref int currentId,DialogScriptableObject data)
        {
            if (data is IDialogFinder finderList) {
                var root = new List<TreeViewItemData<DialogScriptableObject>>();
                foreach (var dataItem in finderList.GetAllData()) {
                    root.Add(GetTreeViewItems(ref currentId, dataItem));
                }
                return new TreeViewItemData<DialogScriptableObject>(currentId++, data, root);
            }
            return new TreeViewItemData<DialogScriptableObject>(currentId++, data);
        }
        public new class UxmlFactory : UxmlFactory<PlotEditorTreeView, TreeView.UxmlTraits> {}
        public Action<DialogScriptableObject> OnSelectionChanged;
        public PlotEditorTreeView()
        {
            BuildTree();
            makeItem = () => new Label() {
                style = {
                    unityTextAlign = TextAnchor.MiddleLeft,
                    flexGrow = 1
                }
            };
            bindItem = (element, index) => {
                DialogScriptableObject data = GetItemDataForIndex<DialogScriptableObject>(index);
                if (data is IDialogIdentifier identifier) {
                    (element as Label).text = identifier.Id;
                }
                data.OnDataChanged = () => {
                    RefreshItem(index);
                };
                data.OnDataHardChanged = RebuildTree;
            };
            selectionChanged += SelectionChanged;
        }
        private void RebuildTree() {
            BuildTree();
            Rebuild();
        }
        private void BuildTree() {
            int id = 0;
            List<TreeViewItemData<DialogScriptableObject>> rootItems = new List<TreeViewItemData<DialogScriptableObject>>();
            foreach (var plot in SDAManager.Instance.GetAllData()) {
                rootItems.Add(GetTreeViewItems(ref id, plot));
            }
            SetRootItems(rootItems);
        }
        private void SelectionChanged(IEnumerable<object> items) {
            var item = items.GetEnumerator();
            if (!item.MoveNext()) return;
            var data = item.Current;
            if (data is DialogScriptableObject dialogScriptableObject) {
                OnSelectionChanged?.Invoke(dialogScriptableObject);
            }
        }
    }
}