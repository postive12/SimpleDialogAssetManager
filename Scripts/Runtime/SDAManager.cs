using System;
using System.Collections.Generic;
using DialogSystem.Runtime.Attributes;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using DialogSystem.Runtime.Structure.ScriptableObjects.Interface;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace DialogSystem.Runtime
{
    public class SDAManager : ScriptableObject
    {
        private const string BASE_PATH = "Assets/Resources";
        private const string DATA_PATH = "Dialogs";
        private const string PLOT_PATH = "Plots";
        private const string GROUP_PATH = "Groups";
        private static readonly List<string> STANDARD_TARGETS = new List<string> {
            "===Base Target===",
            "DialogPlotSelector",
        }; 
        public static SDAManager Instance {
            get {
                if (_instance == null) {
                    var loadedData = Resources.LoadAll<SDAManager>("");
                    _instance = loadedData.Length > 0 ? loadedData[0] : CreateInstance<SDAManager>();
                    _instance.Load();
                }
                return _instance;
            }
        }
        private static SDAManager _instance;
        public static List<string> DialogTargetIds {
            get {
                var result = new List<string>();
                if (Instance != null) {
                    result.AddRange(Instance._dialogTargetIds);
                }
                result.AddRange(STANDARD_TARGETS);
                return result;
            }
        }
        public static List<string> DialogPaths {
            get {
                var result = new List<string>();
                if (Instance != null) {
                    result.AddRange(GetAllDialogPaths("", Instance._dialogDB));
                }
                return result;
            }
        }
        private static List<string> GetAllDialogPaths(string currentPath,DialogScriptableObject data) {
            List<string> result = new List<string>();
            currentPath = string.IsNullOrEmpty(currentPath) ? currentPath : currentPath + "/";
            if (data is IDialogFinder finder) {
                foreach (var plot in finder.DataList) {
                    result.AddRange(GetAllDialogPaths(currentPath + data.Id, plot));
                }
            }
            else {
                result.Add(currentPath + data.Id);
            }
            return result;
        }
        public DialogDB DB => _dialogDB;
        [DialogSelector][SerializeField] private string _id = "";
        [SDAMReadOnly][SerializeField] private DialogDB _dialogDB;
        [SerializeField] private List<string> _dialogTargetIds = new List<string>();
        [SerializeField] private DialogScriptableObject _testTarget;
        [Button]
        private void Test() {
            var result = FindDataOwnerByGuid(_testTarget.GUID, _dialogDB);
            Debug.Log(result.Id);
        }
        public DialogPlotGraph FindDialogPlot(string id) {
            return _dialogDB.FindDialogPlot(id);
        }
        public DialogScriptableObject FindDialogByGuid(string guid,DialogScriptableObject data) {
            if (data.GUID == guid) {
                return data;
            }
            if (data is not IDialogFinder finder) {
                return null;
            }
            foreach (var child in finder.DataList) {
                var result = FindDialogByGuid(guid, child);
                if (result == null) continue;
                return result;
            }
            return null;
        }
        public DialogScriptableObject FindDataOwnerByGuid(string guid,IDialogFinder finder) {
            foreach (var data in finder.DataList) {
                if (data.GUID == guid) {
                    return finder as DialogScriptableObject;
                }
                if (data is IDialogFinder dialogFinder) {
                    var result = FindDataOwnerByGuid(guid, dialogFinder);
                    if (result != null) {
                        return result;
                    }
                }
            }
            return null;
        }
        #if UNITY_EDITOR
        private void Load()
        {
            var loadedData = Resources.LoadAll<DialogDB>("");
            _dialogDB = loadedData.Length > 0 ? loadedData[0] : null;
            if (_dialogDB == null) {
                _dialogDB = CreateInstance<DialogDB>();
                _dialogDB.Id = string.Empty;
                _dialogDB.name = "DialogDB";
                EditorUtility.SetDirty(_dialogDB);
                AssetDatabase.AddObjectToAsset(_dialogDB, this);
                AssetDatabase.SaveAssets();
            }
            CheckAndCreatePath();
        }
        private void CheckAndCreatePath() {
            if (!AssetDatabase.IsValidFolder(BASE_PATH + "/" + DATA_PATH)) {
                AssetDatabase.CreateFolder(BASE_PATH, DATA_PATH);
            }
            if (!AssetDatabase.IsValidFolder(BASE_PATH + "/" + DATA_PATH + "/" + PLOT_PATH)) {
                AssetDatabase.CreateFolder(BASE_PATH + "/" + DATA_PATH, PLOT_PATH);
            }
            if (!AssetDatabase.IsValidFolder(BASE_PATH + "/" + DATA_PATH + "/" + GROUP_PATH)) {
                AssetDatabase.CreateFolder(BASE_PATH + "/" + DATA_PATH, GROUP_PATH);
            }
        }
        #region Editor
        public void AddData(SDAMDataType type,DialogScriptableObject parent = null) {
            CheckAndCreatePath();
            DialogScriptableObject result = null;
            switch (type) {
                case SDAMDataType.PLOT:
                    result = CreateInstance<DialogPlotGraph>();
                    break;
                case SDAMDataType.GROUP:
                    result = CreateInstance<DialogPlotGroup>();
                    break;
                case SDAMDataType.DB:
                    Debug.LogError("DB must be created only once");
                    return;
            }
            if (result == null) {
                return;
            }
            //save data to path
            string path = BASE_PATH + "/" + DATA_PATH;
            switch (type) {
                case SDAMDataType.PLOT:
                    path += "/" + PLOT_PATH;
                    break;
                case SDAMDataType.GROUP:
                    path += "/" + GROUP_PATH;
                    break;
            }
            string hash = Guid.NewGuid().ToString().Substring(0, 8);
            string name = "";
            if (parent != null) {
                name = parent.Id + " - ";
            }
            name += $"New ({hash})";
            result.Id = name;
            result.name = hash;
            //save to path
            if (parent == null) {
                _dialogDB.DataList.Add(result);
                EditorUtility.SetDirty(_dialogDB);
            }
            else {
                if (parent is IDialogFinder finder) {
                    finder.DataList.Add(result);
                    EditorUtility.SetDirty(parent);
                }
            }
            AssetDatabase.CreateAsset(result, path + "/" + hash + ".asset");
            AssetDatabase.SaveAssets();
        }
        public void DeleteData(DialogScriptableObject data) {
            if (data is DialogDB) {
                Debug.LogError("Can't remove DB");
                return;
            }
            var findOwner = FindDataOwnerByGuid(data.GUID, _dialogDB);
            if (findOwner == null) {
                Debug.LogError("Can't find owner");
                return;
            }
            if (findOwner is IDialogFinder finder) {
                finder.DataList.Remove(data);
            }
            AssetDatabase.RemoveObjectFromAsset(data);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        #endregion
        
        #endif
    }
}