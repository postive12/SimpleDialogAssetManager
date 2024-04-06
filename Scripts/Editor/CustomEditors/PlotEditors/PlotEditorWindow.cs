using System.Text;
using DialogSystem.Runtime;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogSystem.Editor.CustomEditors.PlotEditors
{
    public class PlotEditorWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;
        private DialogScriptableObject _currentSelectedData;
        private PlotGraphView _plotGraphView;
        private PlotEditorInspectorView _plotEditorInspectorView;
        [MenuItem("Window/SDAM Plot Editor Window")]
        public static void OpenWindow()
        {
            PlotEditorWindow wnd = GetWindow<PlotEditorWindow>();
            wnd.titleContent = new GUIContent("Plot Editor");
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var data = EditorUtility.InstanceIDToObject(instanceID);
            if (data is DialogScriptableObject or SDAManager && AssetDatabase.CanOpenAssetInEditor(instanceID)) {
                OpenWindow();
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
                if (_currentSelectedData == null) {
                    nameTextField.value = string.Empty;
                    return;
                }
                _currentSelectedData.Id = evt.newValue;
            });
            var treeView = root.Q<PlotEditorTreeView>("plot-tree");
            treeView.OnSelectionChanged = (data) => {
                if (data == null) {
                    _plotGraphView.ClearGraph();
                    _plotEditorInspectorView.Clear();
                    nameTextField.value = string.Empty;
                    _currentSelectedData = null;
                    return;
                }
                _currentSelectedData = data;
                nameTextField.value = data.Id;
                if (data is DialogPlotGraph plot) {
                    _plotGraphView.PopulateView(plot);
                }
                else {
                    _plotGraphView.ClearGraph();
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
            if (_plotGraphView.Plot == null) return;
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

