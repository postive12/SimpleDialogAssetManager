using System.Collections.Generic;
using DialogSystem.Dialogs.Components.Managers;
using UnityEditor;

namespace DialogSystem.Editor.Components
{
    [CustomEditor(typeof(StandAloneDialogManager))]
    public class IndependentDialogManagerEditor : UnityEditor.Editor
    {
        private StandAloneDialogManager _target;
        private void OnEnable() {
            _target = (StandAloneDialogManager)serializedObject.targetObject;
        }
        public override void OnInspectorGUI()
        {
            List<string> excludeMember = new List<string>(){"m_Script"};
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            if (!_target.UseStartUpPlot) {
                excludeMember.Add("_startUpPlotId");
            }
            DrawPropertiesExcluding(serializedObject, excludeMember.ToArray());
            serializedObject.ApplyModifiedProperties();
        }
    }
}