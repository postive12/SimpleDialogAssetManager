using System;
using System.Text;
using DialogSystem.Editor.Windows;
using DialogSystem.Runtime.Dialogs;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlotEditorWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    [SerializeField] private PlotGraphView _plotGraphView;
    [SerializeField] private PlotEditorInspectorView _plotEditorInspectorView;

    [MenuItem("Window/SDAM/Plot Editor Window")]
    public static void OpenWindow()
    {
        PlotEditorWindow wnd = GetWindow<PlotEditorWindow>();
        wnd.titleContent = new GUIContent("PlotEditorWindow");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;
        //find asset with name "PlotEditorWindow.uxml"
        m_VisualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Plugins/SimpleDialogAssetManager/Scripts/Editor/Windows/PlotEditorWindow.uxml");
        m_VisualTreeAsset.CloneTree(root);
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Plugins/SimpleDialogAssetManager/Scripts/Editor/Windows/PlotEditorWindow.uss");
        root.styleSheets.Add(styleSheet);
        
        _plotGraphView = root.Q<PlotGraphView>();
        _plotEditorInspectorView = root.Q<PlotEditorInspectorView>();
        
        root.Q<Button>("save").clicked += Save;
        root.Q<Button>("return").clicked += () => {
            _plotGraphView.ReturnToStartNode();
        };
        root.Q<Button>("sort").clicked += () => {
            _plotGraphView.SortNodes();
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

    private void Save() {
        StringBuilder log = new StringBuilder();
        log.AppendLine("Save Plot : " + _plotGraphView.Plot.name);
        log.AppendLine("Path : " + AssetDatabase.GetAssetPath(_plotGraphView.Plot));
        Debug.Log(log.ToString());
        //save project
        AssetDatabase.SaveAssets();
    }

    private void OnNodeSelectionChange(DLNodeView dlNodeView) {
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
