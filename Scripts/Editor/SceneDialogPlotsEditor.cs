using System;
using System.Collections.Generic;
using DialogSystem.Structure.ScriptableObjects;
using UnityEditor;

namespace DialogSystem.Editor
{
    [CustomEditor(typeof(SceneDialogPlots))]
    public class SceneDialogPlotsEditor : UnityEditor.Editor
    {
        private SceneDialogPlots _target;
        private void OnEnable() {
            _target = (SceneDialogPlots)serializedObject.targetObject;
        }
        public override void OnInspectorGUI()
        {
            List<string> excludeMemeber = new List<string>(){"m_Script"};
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            if (!_target.UseStartUpPlot) {
                excludeMemeber.Add("_startUpPlotId");
            }
            DrawPropertiesExcluding(serializedObject, excludeMemeber.ToArray());
            serializedObject.ApplyModifiedProperties();
        }
        
    }
}