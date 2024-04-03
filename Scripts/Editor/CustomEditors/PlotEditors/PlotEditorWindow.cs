using System;
using System.Collections.Generic;
using System.Text;
using DialogSystem.Editor.CustomEditors;
using DialogSystem.Runtime;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using DialogSystem.Runtime.Structure.ScriptableObjects.Interface;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;
using TreeView = UnityEngine.UIElements.TreeView;

namespace DialogSystem.Editor.CustomEditors.PlotEditors
{
    public class PlotEditorWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;
        private DialogScriptableObject _currentSelectedData;
        private PlotGraphView _plotGraphView;
        private PlotEditorInspectorView _plotEditorInspectorView;
        [MenuItem("Window/SDAM/Plot Editor Window")]
        public static void OpenWindow()
        {
            PlotEditorWindow wnd = GetWindow<PlotEditorWindow>();
            wnd.titleContent = new GUIContent("PlotEditorWindow");
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            DialogPlotGraph dialog = EditorUtility.InstanceIDToObject(instanceID) as DialogPlotGraph;
            if (dialog != null && AssetDatabase.CanOpenAssetInEditor(dialog.GetInstanceID())) {
                PlotEditorWindow.OpenWindow();
                return true;
            }
            return false;
        }
        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;
            //find asset with name "PlotEditorWindow.uxml"
            m_VisualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Assets/Plugins/SimpleDialogAssetManager/Scripts/Editor/CustomEditors/PlotEditors/PlotEditorWindow.uxml");
            m_VisualTreeAsset.CloneTree(root);
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                "Assets/Plugins/SimpleDialogAssetManager/Scripts/Editor/CustomEditors/PlotEditors/PlotEditorWindow.uss");
            root.styleSheets.Add(styleSheet);

            _plotGraphView = root.Q<PlotGraphView>();
            _plotEditorInspectorView = root.Q<PlotEditorInspectorView>();

            root.Q<Button>("save").clicked += Save;
            root.Q<Button>("return").clicked += () => { _plotGraphView.ReturnToStartNode(); };
            root.Q<Button>("sort").clicked += () => { _plotGraphView.SortNodes();};
            var nameTextField = root.Q<TextField>("plot-name");
            nameTextField.RegisterValueChangedCallback(evt => {
                if (_currentSelectedData == null) return;
                _currentSelectedData.Id = evt.newValue;
            });
            var treeView = root.Q<PlotEditorTreeView>("plot-tree");
            treeView.OnSelectionChanged = (data) => {
                _currentSelectedData = data;
                nameTextField.value = data.Id;
                if (data is DialogPlotGraph plot) {
                    _plotGraphView.PopulateView(plot);
                }
            };
            //ctrl+s save shortcut
            root.RegisterCallback<KeyDownEvent>(evt => {
                if (evt.keyCode == KeyCode.S && evt.ctrlKey) {
                    Save();
                }
            });
            _plotGraphView.OnNodeSelectionChanged += OnNodeSelectionChange;
            OnSelectionChange();
        }
        private void Save()
        {
            StringBuilder log = new StringBuilder();
            log.AppendLine("Save Plot : " + _plotGraphView.Plot.name);
            log.AppendLine("Path : " + AssetDatabase.GetAssetPath(_plotGraphView.Plot));
            Debug.Log(log.ToString());
            //save project
            AssetDatabase.SaveAssets();
        }
        private void OnNodeSelectionChange(DLNodeView dlNodeView)
        {
            _plotEditorInspectorView.UpdateSelection(dlNodeView);
        }

        private void OnSelectionChange()
        {
            // When the selection changes, we need to update the inspector
            _plotEditorInspectorView.Clear();
            DialogPlotGraph plot = Selection.activeObject as DialogPlotGraph;
            if (plot != null) {
                Debug.Log("Selected plot : " + plot.name);
                _plotGraphView.PopulateView(plot);
            }
        }
    }
}

