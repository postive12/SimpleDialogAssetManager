using System;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Structure;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
namespace DialogSystem.Nodes
{
    public abstract class DialogBaseNode : ScriptableObject
    {
        public DialogType Type { get; protected set; } = DialogType.DIALOG;
        public string Guid => _guid;
        public Vector2 Position {
            get => _position;
            set {
                _position = value;
                //set dirty
                #if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
                #endif
            }
        }
        [HideInInspector][SerializeField] private string _guid = "";
        [HideInInspector][SerializeField] private Vector2 _position = new Vector2(0,0);

        public bool IsPlayed {
            get => _isPlayed;
            set => _isPlayed = value;
        }
        private bool _isPlayed = false;

        public DialogBaseNode() {
            if (_guid.IsNullOrWhitespace()) {
                _guid = System.Guid.NewGuid().ToString();
            }
        }
        public abstract DialogBaseNode GetNext();
        public abstract bool IsNextExist();
        public abstract void Play(IDialogManager target);
        public abstract bool CanGetNext();
        public virtual void ResetNode() { }
        public virtual void CheckIntegrity() { }

        public virtual void OnValidate() {
            CheckIntegrity();
        }
    }
}