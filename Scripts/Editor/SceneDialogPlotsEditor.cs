using DialogSystem.Structure.ScriptableObjects;
using UnityEditor;

namespace DialogSystem.Editor
{
    [CustomEditor(typeof(SceneDialogPlots))]
    public class SceneDialogPlotsEditor : UnityEditor.Editor
    {
        private static readonly string[] _excludeMemeber = new string[]{"m_Script"};
        private bool _isStartUpExist = false;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            foreach (var member in _excludeMemeber) {
                DrawPropertiesExcluding(serializedObject, member);
            }
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }
        
    }
}