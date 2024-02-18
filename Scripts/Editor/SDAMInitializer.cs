﻿using DialogSystem;
using DialogSystem.Runtime.Dialogs;
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

        public static void CheckAndCreateAssetFolder()
        {
            //check resource folder is exist
            if (!AssetDatabase.IsValidFolder(SDAMConst.SDAM_ASSET_BASE_FOLDER)) {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
            //check dialog folder is exist
            if (!AssetDatabase.IsValidFolder(SDAMConst.SDAM_ASSET_BASE_FOLDER + SDAMConst.SDAM_ASSET_FOLDER)) {
                AssetDatabase.CreateFolder("Assets/Resources", "Dialogs");
            }
            #if UNITY_EDITOR
                Debug.Log("SDAM: Asset folder is ready!");
            #endif
        }
    }
}