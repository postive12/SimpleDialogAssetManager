using DialogSystem.Runtime.Structure.ScriptableObjects;
using UnityEditor;
namespace DialogSystem.Editor
{
    [CustomEditor(typeof(DialogPlotGroup))]
    public class DialogPlotSetEditor : UnityEditor.Editor
    {
        private static readonly string[] _excludeMemeber = new string[]{"m_Script"};
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