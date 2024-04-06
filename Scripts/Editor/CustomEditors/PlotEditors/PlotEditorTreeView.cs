using System;
using System.Collections.Generic;
using DialogSystem.Runtime;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using DialogSystem.Runtime.Structure.ScriptableObjects.Interface;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogSystem.Editor.CustomEditors.PlotEditors
{
    public class PlotEditorTreeView : TreeView
    {
        private static TreeViewItemData<DialogScriptableObject> GetTreeViewItems(ref int currentId,DialogScriptableObject data)
        {
            if (data is IDialogFinder finder) {
                var root = new List<TreeViewItemData<DialogScriptableObject>>();
                foreach (var dataItem in finder.DataList) {
                    root.Add(GetTreeViewItems(ref currentId, dataItem));
                }
                return new TreeViewItemData<DialogScriptableObject>(currentId++, data, root);
            }
            return new TreeViewItemData<DialogScriptableObject>(currentId++, data);
        }
        public new class UxmlFactory : UxmlFactory<PlotEditorTreeView, TreeView.UxmlTraits> {}
        public Action<DialogScriptableObject> OnSelectionChanged;
        private DialogScriptableObject _selectedData = null;
        public PlotEditorTreeView()
        {
            BuildTree();
            makeItem = () => new Label() {
                style = {
                    unityTextAlign = TextAnchor.MiddleLeft,
                    flexGrow = 1
                },
            };
            bindItem = (element, index) => {
                DialogScriptableObject data = GetItemDataForIndex<DialogScriptableObject>(index);
                if (data is IDialogIdentifier identifier) {
                    (element as Label).text = identifier.Id + $" ({data.SDAMDataType.ToString()})";
                }
                data.OnDataChanged = () => {
                    RefreshItem(index);
                };
            };
            selectionChanged += SelectionChanged;
            this.AddManipulator(new ContextualMenuManipulator(BuildContextMenu));
        }
        private void RebuildTree() {
            BuildTree();
            Rebuild();
        }
        private void BuildTree() {
            int id = 0;
            List<TreeViewItemData<DialogScriptableObject>> rootItems = new List<TreeViewItemData<DialogScriptableObject>>();
            foreach (var data in SDAManager.Instance.DB.DataList) {
                rootItems.Add(GetTreeViewItems(ref id, data));
            }
            SetRootItems(rootItems);
        }
        private void BuildContextMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction(_selectedData == null ? "Target : Root":  $"Target : {_selectedData.Id}",null);
            if (_selectedData != null) {
                //show file name
                evt.menu.AppendAction("File Name : " + _selectedData.name+"(Find)", (action) => {
                    //select file in project window
                    EditorGUIUtility.PingObject(_selectedData);
                }, DropdownMenuAction.AlwaysEnabled);
            }
            evt.menu.AppendSeparator();
            if (_selectedData == null || _selectedData.SDAMDataType == SDAMDataType.GROUP) {
                evt.menu.AppendAction("Add Child/Add Group", (action) => {
                    SDAManager.Instance.AddData(SDAMDataType.GROUP,_selectedData);
                    RebuildTree();
                });
                evt.menu.AppendAction("Add Child/Add Plot", (action) => {
                    SDAManager.Instance.AddData(SDAMDataType.PLOT,_selectedData);
                    RebuildTree();
                });
            }
            if (_selectedData != null) {
                evt.menu.AppendAction("Delete", (action) => {
                    SDAManager.Instance.DeleteData(_selectedData);
                    _selectedData = null;
                    OnSelectionChanged?.Invoke(null);
                    RebuildTree();
                });
            }

        }

        private void SelectionChanged(IEnumerable<object> items) {
            var item = items.GetEnumerator();
            if (!item.MoveNext()) return;
            var data = item.Current;
            if (data is DialogScriptableObject dialogScriptableObject) {
                _selectedData = dialogScriptableObject;
                OnSelectionChanged?.Invoke(dialogScriptableObject);
            }
        }
        
    }
}