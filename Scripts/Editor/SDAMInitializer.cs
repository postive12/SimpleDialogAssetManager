using DialogSystem.Editor.CustomEditors;
using DialogSystem.Editor.CustomEditors.PlotEditors;
using DialogSystem.Runtime;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace DialogSystem.Editor
{
    [InitializeOnLoad]
    public class SDAMInitializer
    {
        static SDAMInitializer() {
            CheckAndCreateAssetFolder();
            Selection.selectionChanged += OnSelectionChanged;
        }

        private static void OnSelectionChanged()
        {
            DialogPlotGraph dialog = Selection.activeObject as DialogPlotGraph;
            if(dialog != null && AssetDatabase.CanOpenAssetInEditor(dialog.GetInstanceID())) {
                PlotEditorWindow.OpenWindow();
            }
        }
        private static void CheckAndCreateAssetFolder()
        {
            //check resource folder is exist
            SDAManager manager = AssetDatabase.LoadAssetAtPath<SDAManager>("Assets/Resources/SDAMSettings.asset");
            if (!manager) {
                manager = ScriptableObject.CreateInstance<SDAManager>();
                AssetDatabase.CreateAsset(manager, "Assets/Resources/SDAMSettings.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log("SDAM: SDAMSettings is ready!");
            }
        }
    }
}